using RetailSales.DTO;

namespace RetailSales.DAL
{
    public class HoaDonDAL
    {
        // Lập hóa đơn trong 1 SqlTransaction (MySqlTransaction):
        //  1. Chèn HoaDon
        //  2. Chèn các ChiTietHoaDon
        //  3. Trừ SoLuongTon (không đủ hàng -> Rollback và báo lỗi)
        //  4. Cộng điểm tích lũy cho khách hàng
        //  5. Commit
        public int Save(HoaDonDTO hd)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                int maHD;

                // Bước 1
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "INSERT INTO HoaDon(NgayLap, MaNV, MaKH, TongTien) VALUES(NOW(), @nv, @kh, @tong)",
                    ("@nv", hd.MaNV), ("@kh", hd.MaKH), ("@tong", hd.TongTien)))
                {
                    cmd.ExecuteNonQuery();
                    maHD = (int)cmd.LastInsertedId;
                }

                // Bước 2
                foreach (var ct in hd.ChiTiet)
                {
                    using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                        "INSERT INTO ChiTietHoaDon(MaHD, MaSP, SoLuong, DonGia) VALUES(@hd, @sp, @sl, @dg)",
                        ("@hd", maHD), ("@sp", ct.MaSP), ("@sl", ct.SoLuong), ("@dg", ct.DonGia)))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // Bước 3
                foreach (var ct in hd.ChiTiet)
                {
                    int ton;
                    using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                        "SELECT SoLuongTon FROM SanPham WHERE MaSP=@sp FOR UPDATE", ("@sp", ct.MaSP)))
                    {
                        object o = cmd.ExecuteScalar();
                        if (o == null)
                            throw new InvalidOperationException("Sản phẩm " + ct.MaSP + " không tồn tại.");
                        ton = Convert.ToInt32(o);
                    }

                    // Nếu SoLuongTon < SoLuong bán -> báo lỗi, khối catch bên dưới sẽ Rollback
                    if (ton < ct.SoLuong)
                        throw new InvalidOperationException(
                            "Không đủ hàng trong kho: " + ct.MaSP + " (tồn " + ton + ", cần bán " + ct.SoLuong + ").");

                    using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                        "UPDATE SanPham SET SoLuongTon = SoLuongTon - @sl WHERE MaSP=@sp",
                        ("@sl", ct.SoLuong), ("@sp", ct.MaSP)))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // Bước 4
                if (hd.MaKH != null && hd.DiemCong > 0)
                {
                    using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                        "UPDATE KhachHang SET DiemTichLuy = DiemTichLuy + @d WHERE MaKH=@kh",
                        ("@d", hd.DiemCong), ("@kh", hd.MaKH)))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // Bước 5
                tran.Commit();
                return maHD;
            }
            catch
            {
                try { tran.Rollback(); } catch { }
                throw;
            }
        }

        // ExecuteScalar: tổng doanh thu trong ngày
        public decimal GetDoanhThuNgay(DateTime ngay)
        {
            object o = DatabaseHelper.ExecuteScalar(
                "SELECT IFNULL(SUM(TongTien), 0) FROM HoaDon WHERE DATE(NgayLap) = @d",
                ("@d", ngay.Date));
            return Convert.ToDecimal(o);
        }

        // ExecuteScalar: tổng doanh thu trong tháng
        public decimal GetDoanhThuThang(int thang, int nam)
        {
            object o = DatabaseHelper.ExecuteScalar(
                "SELECT IFNULL(SUM(TongTien), 0) FROM HoaDon WHERE MONTH(NgayLap) = @m AND YEAR(NgayLap) = @y",
                ("@m", thang), ("@y", nam));
            return Convert.ToDecimal(o);
        }

        // SqlDataReader (MySqlDataReader): Top sản phẩm bán chạy nhất
        public List<TopSanPhamDTO> GetTopSanPham(int top)
        {
            var list = new List<TopSanPhamDTO>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = DatabaseHelper.CreateCommand(conn, null,
                "SELECT sp.MaSP, sp.TenSP, SUM(ct.SoLuong) AS TongSoLuong, SUM(ct.SoLuong * ct.DonGia) AS DoanhThu " +
                "FROM ChiTietHoaDon ct JOIN SanPham sp ON sp.MaSP = ct.MaSP " +
                "GROUP BY sp.MaSP, sp.TenSP ORDER BY TongSoLuong DESC LIMIT @top",
                ("@top", top));
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new TopSanPhamDTO
                {
                    MaSP = rd["MaSP"].ToString(),
                    TenSP = rd["TenSP"].ToString(),
                    TongSoLuong = Convert.ToInt32(rd["TongSoLuong"]),
                    DoanhThu = Convert.ToDecimal(rd["DoanhThu"])
                });
            }
            return list;
        }
    }
}

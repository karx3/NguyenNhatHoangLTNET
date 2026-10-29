using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class PhieuKhoDAL
    {
        // Lưu phiếu kho trong 1 SqlTransaction (MySqlTransaction):
        //  1. Chèn PhieuKho
        //  2. Chèn ChiTietPhieuKho
        //  3. Cập nhật tồn kho (NHAP: cộng, XUAT: kiểm tra đủ hàng rồi trừ)
        //  4. Commit khi thành công, Rollback khi có lỗi
        public int Save(PhieuKhoDTO p)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                int maPhieu;

                // Bước 1
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "INSERT INTO PhieuKho(LoaiPhieu, NgayLap, MaNCC, TongTien) VALUES(@loai, NOW(), @ncc, @tong)",
                    ("@loai", p.LoaiPhieu), ("@ncc", p.MaNCC), ("@tong", p.TongTien)))
                {
                    cmd.ExecuteNonQuery();
                    maPhieu = (int)cmd.LastInsertedId;
                }

                foreach (var ct in p.ChiTiet)
                {
                    // Bước 2
                    using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                        "INSERT INTO ChiTietPhieuKho(MaPhieu, MaVatTu, SoLuong, DonGia) VALUES(@p, @vt, @sl, @dg)",
                        ("@p", maPhieu), ("@vt", ct.MaVatTu), ("@sl", ct.SoLuong), ("@dg", ct.DonGia)))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Bước 3
                    if (p.LoaiPhieu == "NHAP")
                    {
                        int rows;
                        using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                            "UPDATE VatTu SET SoLuongTon = SoLuongTon + @sl WHERE MaVatTu=@vt",
                            ("@sl", ct.SoLuong), ("@vt", ct.MaVatTu)))
                        {
                            rows = cmd.ExecuteNonQuery();
                        }
                        if (rows == 0)
                            throw new InvalidOperationException("Vật tư " + ct.MaVatTu + " không tồn tại.");
                    }
                    else
                    {
                        decimal ton;
                        using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                            "SELECT SoLuongTon FROM VatTu WHERE MaVatTu=@vt FOR UPDATE",
                            ("@vt", ct.MaVatTu)))
                        {
                            object o = cmd.ExecuteScalar();
                            if (o == null)
                                throw new InvalidOperationException("Vật tư " + ct.MaVatTu + " không tồn tại.");
                            ton = Convert.ToDecimal(o);
                        }

                        // Kiểm tra tồn kho: nếu SoLuongTon < SoLuong xuất thì báo lỗi và Rollback
                        if (ton < ct.SoLuong)
                            throw new InvalidOperationException(
                                "Không đủ hàng trong kho: " + ct.MaVatTu +
                                " (tồn " + ton.ToString("N2") + ", cần xuất " + ct.SoLuong.ToString("N2") + ").");

                        using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                            "UPDATE VatTu SET SoLuongTon = SoLuongTon - @sl WHERE MaVatTu=@vt",
                            ("@sl", ct.SoLuong), ("@vt", ct.MaVatTu)))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // Bước 4
                tran.Commit();
                return maPhieu;
            }
            catch
            {
                try { tran.Rollback(); } catch { }
                throw;
            }
        }
    }
}

using System.Data;
using HotelManagement.DTO;

namespace HotelManagement.DAL
{
    public class PhieuDAL
    {
        // Danh sách phiếu đang lưu trú (chưa trả phòng)
        public List<PhieuDTO> GetActive()
        {
            var list = new List<PhieuDTO>();
            DataTable dt = DatabaseHelper.GetTable(
                "SELECT p.MaPhieu, p.SoPhong, p.MaKH, k.HoTen, p.NgayNhanPhong, ph.GiaTheoGio " +
                "FROM PhieuDatPhong p " +
                "JOIN KhachHang k ON k.MaKH = p.MaKH " +
                "JOIN Phong ph ON ph.SoPhong = p.SoPhong " +
                "WHERE p.NgayTraPhong IS NULL ORDER BY p.SoPhong");
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new PhieuDTO
                {
                    MaPhieu = Convert.ToInt32(r["MaPhieu"]),
                    SoPhong = r["SoPhong"].ToString(),
                    MaKH = Convert.ToInt32(r["MaKH"]),
                    TenKH = r["HoTen"].ToString(),
                    NgayNhanPhong = Convert.ToDateTime(r["NgayNhanPhong"]),
                    GiaTheoGio = Convert.ToDecimal(r["GiaTheoGio"])
                });
            }
            return list;
        }

        // Check-in: thêm khách mới (nếu cần) + lập phiếu + chuyển phòng sang "Đang ở" trong 1 transaction
        public int CheckIn(KhachHangDTO kh, string soPhong)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                int maKH = kh.MaKH;
                if (maKH == 0)
                {
                    using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                        "INSERT INTO KhachHang(HoTen, CCCD, SoDienThoai) VALUES(@ten, @cccd, @sdt)",
                        ("@ten", kh.HoTen), ("@cccd", kh.CCCD), ("@sdt", kh.SoDienThoai)))
                    {
                        cmd.ExecuteNonQuery();
                        maKH = (int)cmd.LastInsertedId;
                    }
                }

                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "SELECT TrangThai FROM Phong WHERE SoPhong=@p FOR UPDATE", ("@p", soPhong)))
                {
                    object o = cmd.ExecuteScalar();
                    if (o == null || o.ToString() != "Trống")
                        throw new InvalidOperationException("Phòng này không còn trống.");
                }

                int maPhieu;
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "INSERT INTO PhieuDatPhong(SoPhong, MaKH, NgayNhanPhong, TongTienThanhToan) VALUES(@p, @k, NOW(), 0)",
                    ("@p", soPhong), ("@k", maKH)))
                {
                    cmd.ExecuteNonQuery();
                    maPhieu = (int)cmd.LastInsertedId;
                }

                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "UPDATE Phong SET TrangThai='Đang ở' WHERE SoPhong=@p", ("@p", soPhong)))
                {
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                return maPhieu;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public int AddService(int maPhieu, int maDV, int soLuong)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO SuDungDichVu(MaPhieu, MaDV, SoLuong, ThoiGian) VALUES(@p, @dv, @sl, NOW())",
                ("@p", maPhieu), ("@dv", maDV), ("@sl", soLuong));
        }

        public DataTable GetServiceTable(int maPhieu)
        {
            return DatabaseHelper.GetTable(
                "SELECT d.TenDV, s.SoLuong, d.DonGia, s.SoLuong * d.DonGia AS ThanhTien, s.ThoiGian " +
                "FROM SuDungDichVu s JOIN DichVu d ON d.MaDV = s.MaDV " +
                "WHERE s.MaPhieu=@p ORDER BY s.ThoiGian",
                ("@p", maPhieu));
        }

        public decimal GetServiceTotal(int maPhieu)
        {
            object o = DatabaseHelper.ExecuteScalar(
                "SELECT IFNULL(SUM(s.SoLuong * d.DonGia), 0) FROM SuDungDichVu s " +
                "JOIN DichVu d ON d.MaDV = s.MaDV WHERE s.MaPhieu=@p",
                ("@p", maPhieu));
            return Convert.ToDecimal(o);
        }

        // Check-out: lưu tổng tiền, ngày trả, chuyển phòng sang "Đang dọn dẹp" trong 1 transaction
        public void CheckOut(int maPhieu, string soPhong, DateTime ngayTra, decimal tongTien)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                int rows;
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "UPDATE PhieuDatPhong SET NgayTraPhong=@ra, TongTienThanhToan=@tong " +
                    "WHERE MaPhieu=@m AND NgayTraPhong IS NULL",
                    ("@ra", ngayTra), ("@tong", tongTien), ("@m", maPhieu)))
                {
                    rows = cmd.ExecuteNonQuery();
                }
                if (rows == 0)
                    throw new InvalidOperationException("Phiếu này đã được thanh toán trước đó.");

                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "UPDATE Phong SET TrangThai='Đang dọn dẹp' WHERE SoPhong=@p", ("@p", soPhong)))
                {
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}

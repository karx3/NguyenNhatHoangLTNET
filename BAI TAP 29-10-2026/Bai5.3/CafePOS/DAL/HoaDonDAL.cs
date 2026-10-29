using System.Data;
using MySql.Data.MySqlClient;

namespace CafePOS.DAL
{
    public class HoaDonDAL
    {
        // Hóa đơn chưa thanh toán của bàn (0 nếu chưa có)
        public int GetOpenInvoiceId(int maBan)
        {
            object o = DatabaseHelper.ExecuteScalar(
                "SELECT MaHoaDon FROM HoaDon WHERE MaBan=@b AND TrangThai=0 ORDER BY MaHoaDon DESC LIMIT 1",
                ("@b", maBan));
            return (o == null || o == DBNull.Value) ? 0 : Convert.ToInt32(o);
        }

        // Nạp danh sách món đã gọi vào DataTable (mô hình ngắt kết nối)
        public DataTable GetOrderTable(int maHoaDon)
        {
            return DatabaseHelper.GetTable(
                "SELECT m.MaMon, m.TenMon, c.SoLuong, c.DonGia, c.SoLuong * c.DonGia AS ThanhTien " +
                "FROM ChiTietHoaDon c JOIN MonAn m ON m.MaMon = c.MaMon " +
                "WHERE c.MaHoaDon=@h ORDER BY m.TenMon",
                ("@h", maHoaDon));
        }

        // Thêm món: tự tạo hóa đơn nếu bàn chưa có, đổi trạng thái bàn sang "Có khách"
        public void AddItem(int maBan, int maMon, int soLuong)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                int maHD;
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "SELECT MaHoaDon FROM HoaDon WHERE MaBan=@b AND TrangThai=0 ORDER BY MaHoaDon DESC LIMIT 1",
                    ("@b", maBan)))
                {
                    object o = cmd.ExecuteScalar();
                    maHD = (o == null) ? 0 : Convert.ToInt32(o);
                }

                if (maHD == 0)
                {
                    using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                        "INSERT INTO HoaDon(MaBan, GioVao, TongTien, TrangThai) VALUES(@b, NOW(), 0, 0)",
                        ("@b", maBan)))
                    {
                        cmd.ExecuteNonQuery();
                        maHD = (int)cmd.LastInsertedId;
                    }
                }

                decimal gia;
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "SELECT DonGia FROM MonAn WHERE MaMon=@m", ("@m", maMon)))
                {
                    object o = cmd.ExecuteScalar();
                    if (o == null) throw new InvalidOperationException("Món ăn không tồn tại.");
                    gia = Convert.ToDecimal(o);
                }

                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "INSERT INTO ChiTietHoaDon(MaHoaDon, MaMon, SoLuong, DonGia) VALUES(@h, @m, @sl, @g) " +
                    "ON DUPLICATE KEY UPDATE SoLuong = SoLuong + @sl",
                    ("@h", maHD), ("@m", maMon), ("@sl", soLuong), ("@g", gia)))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "UPDATE BanAn SET TrangThai='Có khách' WHERE MaBan=@b", ("@b", maBan)))
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

        // Thanh toán: tính tổng tiền, cập nhật hóa đơn, trả bàn về "Trống" - tất cả trong 1 transaction
        public decimal Pay(int maBan)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                int maHD;
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "SELECT MaHoaDon FROM HoaDon WHERE MaBan=@b AND TrangThai=0 ORDER BY MaHoaDon DESC LIMIT 1",
                    ("@b", maBan)))
                {
                    object o = cmd.ExecuteScalar();
                    maHD = (o == null) ? 0 : Convert.ToInt32(o);
                }
                if (maHD == 0)
                    throw new InvalidOperationException("Bàn này chưa có hóa đơn cần thanh toán.");

                decimal tong;
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "SELECT IFNULL(SUM(SoLuong * DonGia), 0) FROM ChiTietHoaDon WHERE MaHoaDon=@h",
                    ("@h", maHD)))
                {
                    tong = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "UPDATE HoaDon SET GioRa=NOW(), TongTien=@t, TrangThai=1 WHERE MaHoaDon=@h",
                    ("@t", tong), ("@h", maHD)))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "UPDATE BanAn SET TrangThai='Trống' WHERE MaBan=@b", ("@b", maBan)))
                {
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                return tong;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        // Đổi bàn: chuyển hóa đơn chưa thanh toán sang bàn trống khác
        public void MoveTable(int tuBan, int denBan)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "SELECT TrangThai FROM BanAn WHERE MaBan=@b FOR UPDATE", ("@b", denBan)))
                {
                    object o = cmd.ExecuteScalar();
                    if (o == null || o.ToString() != "Trống")
                        throw new InvalidOperationException("Bàn muốn chuyển đến không còn trống.");
                }

                int rows;
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "UPDATE HoaDon SET MaBan=@den WHERE MaBan=@tu AND TrangThai=0",
                    ("@den", denBan), ("@tu", tuBan)))
                {
                    rows = cmd.ExecuteNonQuery();
                }
                if (rows == 0)
                    throw new InvalidOperationException("Bàn hiện tại chưa có hóa đơn để đổi.");

                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "UPDATE BanAn SET TrangThai='Có khách' WHERE MaBan=@b", ("@b", denBan)))
                {
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "UPDATE BanAn SET TrangThai='Trống' WHERE MaBan=@b", ("@b", tuBan)))
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

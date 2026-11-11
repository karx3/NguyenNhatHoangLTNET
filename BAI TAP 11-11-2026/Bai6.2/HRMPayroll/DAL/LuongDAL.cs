using System.Data;
using HRMPayroll.DTO;

namespace HRMPayroll.DAL
{
    public class LuongDAL
    {
        // Dữ liệu đầu vào để tính lương: nhân viên + chấm công trong tháng
        public List<LuongDTO> GetPayrollData(string thangNam)
        {
            var list = new List<LuongDTO>();
            DataTable dt = DatabaseHelper.GetTable(
                "SELECT nv.MaNV, nv.HoTen, nv.LuongCoBan, nv.HeSoLuong, cc.SoNgayCong, cc.SoGioOT " +
                "FROM NhanVien nv JOIN BangChamCong cc ON cc.MaNV = nv.MaNV " +
                "WHERE cc.ThangNam = @t ORDER BY nv.MaNV",
                ("@t", thangNam));
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new LuongDTO
                {
                    MaNV = Convert.ToInt32(r["MaNV"]),
                    HoTen = r["HoTen"].ToString(),
                    LuongCoBan = Convert.ToDecimal(r["LuongCoBan"]),
                    HeSoLuong = Convert.ToDouble(r["HeSoLuong"]),
                    SoNgayCong = Convert.ToDouble(r["SoNgayCong"]),
                    SoGioOT = Convert.ToDouble(r["SoGioOT"])
                });
            }
            return list;
        }

        // Chốt lương: ghi bảng lương cho toàn bộ nhân viên trong tháng trong 1 transaction.
        // Sau khi chốt, tháng đó có dữ liệu trong BangLuong nên chấm công bị khóa.
        public void ChotLuong(string thangNam, List<LuongDTO> ds)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                    "SELECT COUNT(*) FROM BangLuong WHERE ThangNam=@t", ("@t", thangNam)))
                {
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        throw new InvalidOperationException("Tháng " + thangNam + " đã được chốt lương trước đó.");
                }

                foreach (var l in ds)
                {
                    using (var cmd = DatabaseHelper.CreateCommand(conn, tran,
                        "INSERT INTO BangLuong(MaNV, ThangNam, ThucLinh) VALUES(@nv, @t, @tl)",
                        ("@nv", l.MaNV), ("@t", thangNam), ("@tl", l.ThucLinh)))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch
            {
                try { tran.Rollback(); } catch { }
                throw;
            }
        }
    }
}

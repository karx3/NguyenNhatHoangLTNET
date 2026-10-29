using System.Data;
using HotelManagement.DTO;

namespace HotelManagement.DAL
{
    public class KhachHangDAL
    {
        // Tra cứu theo CCCD bằng tham số (SqlParameter) - chống SQL Injection
        public KhachHangDTO GetByCCCD(string cccd)
        {
            DataTable dt = DatabaseHelper.GetTable(
                "SELECT MaKH, HoTen, CCCD, SoDienThoai FROM KhachHang WHERE CCCD=@cccd",
                ("@cccd", cccd));
            if (dt.Rows.Count == 0) return null;
            DataRow r = dt.Rows[0];
            return new KhachHangDTO
            {
                MaKH = Convert.ToInt32(r["MaKH"]),
                HoTen = r["HoTen"].ToString(),
                CCCD = r["CCCD"].ToString(),
                SoDienThoai = r["SoDienThoai"] == DBNull.Value ? "" : r["SoDienThoai"].ToString()
            };
        }
    }
}

using System.Data;
using HotelManagement.DTO;

namespace HotelManagement.DAL
{
    public class PhongDAL
    {
        // Nạp toàn bộ phòng vào DataTable để lọc ngắt kết nối bằng DataView
        public DataTable GetAllTable()
        {
            return DatabaseHelper.GetTable(
                "SELECT SoPhong, LoaiPhong, GiaTheoGio, TrangThai FROM Phong ORDER BY SoPhong");
        }

        public List<PhongDTO> GetByStatus(string trangThai)
        {
            var list = new List<PhongDTO>();
            DataTable dt = DatabaseHelper.GetTable(
                "SELECT SoPhong, LoaiPhong, GiaTheoGio, TrangThai FROM Phong WHERE TrangThai=@tt ORDER BY SoPhong",
                ("@tt", trangThai));
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new PhongDTO
                {
                    SoPhong = r["SoPhong"].ToString(),
                    LoaiPhong = r["LoaiPhong"].ToString(),
                    GiaTheoGio = Convert.ToDecimal(r["GiaTheoGio"]),
                    TrangThai = r["TrangThai"].ToString()
                });
            }
            return list;
        }

        public int UpdateStatus(string soPhong, string trangThai)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "UPDATE Phong SET TrangThai=@tt WHERE SoPhong=@p",
                ("@tt", trangThai), ("@p", soPhong));
        }
    }
}

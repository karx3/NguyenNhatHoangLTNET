using System.Data;
using HotelManagement.DTO;

namespace HotelManagement.DAL
{
    public class DichVuDAL
    {
        public List<DichVuDTO> GetAll()
        {
            var list = new List<DichVuDTO>();
            DataTable dt = DatabaseHelper.GetTable("SELECT MaDV, TenDV, DonGia FROM DichVu ORDER BY TenDV");
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new DichVuDTO
                {
                    MaDV = Convert.ToInt32(r["MaDV"]),
                    TenDV = r["TenDV"].ToString(),
                    DonGia = Convert.ToDecimal(r["DonGia"])
                });
            }
            return list;
        }
    }
}

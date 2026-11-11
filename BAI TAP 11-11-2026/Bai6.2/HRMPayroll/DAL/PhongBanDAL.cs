using System.Data;
using HRMPayroll.DTO;

namespace HRMPayroll.DAL
{
    public class PhongBanDAL
    {
        public List<PhongBanDTO> GetAll()
        {
            var list = new List<PhongBanDTO>();
            DataTable dt = DatabaseHelper.GetTable("SELECT MaPB, TenPB, Mota FROM PhongBan ORDER BY MaPB");
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new PhongBanDTO
                {
                    MaPB = Convert.ToInt32(r["MaPB"]),
                    TenPB = r["TenPB"].ToString(),
                    Mota = DatabaseHelper.Str(r["Mota"])
                });
            }
            return list;
        }
    }
}

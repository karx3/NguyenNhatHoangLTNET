using System.Data;
using CafePOS.DTO;

namespace CafePOS.DAL
{
    public class MonAnDAL
    {
        public DataTable GetTable()
        {
            return DatabaseHelper.GetTable(
                "SELECT m.MaMon, m.TenMon, m.MaDanhMuc, d.TenDanhMuc, m.DonGia " +
                "FROM MonAn m JOIN DanhMuc d ON d.MaDanhMuc = m.MaDanhMuc ORDER BY m.MaMon");
        }

        public List<MonAnDTO> GetByDanhMuc(int maDanhMuc)
        {
            var list = new List<MonAnDTO>();
            DataTable dt = DatabaseHelper.GetTable(
                "SELECT MaMon, TenMon, MaDanhMuc, DonGia FROM MonAn WHERE MaDanhMuc=@dm ORDER BY TenMon",
                ("@dm", maDanhMuc));
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new MonAnDTO
                {
                    MaMon = Convert.ToInt32(r["MaMon"]),
                    TenMon = r["TenMon"].ToString(),
                    MaDanhMuc = Convert.ToInt32(r["MaDanhMuc"]),
                    DonGia = Convert.ToDecimal(r["DonGia"])
                });
            }
            return list;
        }

        public int Insert(MonAnDTO m)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO MonAn(TenMon, MaDanhMuc, DonGia) VALUES(@ten, @dm, @gia)",
                ("@ten", m.TenMon), ("@dm", m.MaDanhMuc), ("@gia", m.DonGia));
        }

        public int Update(MonAnDTO m)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "UPDATE MonAn SET TenMon=@ten, MaDanhMuc=@dm, DonGia=@gia WHERE MaMon=@ma",
                ("@ten", m.TenMon), ("@dm", m.MaDanhMuc), ("@gia", m.DonGia), ("@ma", m.MaMon));
        }

        public int Delete(int maMon)
        {
            return DatabaseHelper.ExecuteNonQuery("DELETE FROM MonAn WHERE MaMon=@ma", ("@ma", maMon));
        }
    }
}

using System.Data;
using CafePOS.DTO;

namespace CafePOS.DAL
{
    public class DanhMucDAL
    {
        public List<DanhMucDTO> GetAll()
        {
            var list = new List<DanhMucDTO>();
            DataTable dt = DatabaseHelper.GetTable("SELECT MaDanhMuc, TenDanhMuc FROM DanhMuc ORDER BY MaDanhMuc");
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new DanhMucDTO
                {
                    MaDanhMuc = Convert.ToInt32(r["MaDanhMuc"]),
                    TenDanhMuc = r["TenDanhMuc"].ToString()
                });
            }
            return list;
        }

        public int Insert(string ten)
        {
            return DatabaseHelper.ExecuteNonQuery("INSERT INTO DanhMuc(TenDanhMuc) VALUES(@ten)", ("@ten", ten));
        }

        public int Update(int ma, string ten)
        {
            return DatabaseHelper.ExecuteNonQuery("UPDATE DanhMuc SET TenDanhMuc=@ten WHERE MaDanhMuc=@ma",
                ("@ten", ten), ("@ma", ma));
        }

        public int Delete(int ma)
        {
            return DatabaseHelper.ExecuteNonQuery("DELETE FROM DanhMuc WHERE MaDanhMuc=@ma", ("@ma", ma));
        }
    }
}

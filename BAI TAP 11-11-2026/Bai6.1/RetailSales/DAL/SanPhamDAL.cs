using System.Data;
using RetailSales.DTO;

namespace RetailSales.DAL
{
    public class SanPhamDAL
    {
        private static SanPhamDTO Map(DataRow r)
        {
            return new SanPhamDTO
            {
                MaSP = r["MaSP"].ToString(),
                TenSP = r["TenSP"].ToString(),
                GiaBan = Convert.ToDecimal(r["GiaBan"]),
                SoLuongTon = Convert.ToInt32(r["SoLuongTon"])
            };
        }

        public DataTable GetAll()
        {
            return DatabaseHelper.GetTable("SELECT MaSP, TenSP, GiaBan, SoLuongTon FROM SanPham ORDER BY MaSP");
        }

        // Tìm theo tên hoặc mã vạch (MaSP) với ký tự đại diện %, dùng tham số chống SQL Injection
        public DataTable Search(string keyword)
        {
            return DatabaseHelper.GetTable(
                "SELECT MaSP, TenSP, GiaBan, SoLuongTon FROM SanPham " +
                "WHERE TenSP LIKE @k OR MaSP LIKE @k ORDER BY MaSP",
                ("@k", "%" + keyword + "%"));
        }

        public List<SanPhamDTO> GetAllList()
        {
            var list = new List<SanPhamDTO>();
            foreach (DataRow r in GetAll().Rows) list.Add(Map(r));
            return list;
        }

        public SanPhamDTO GetByMa(string ma)
        {
            DataTable dt = DatabaseHelper.GetTable(
                "SELECT MaSP, TenSP, GiaBan, SoLuongTon FROM SanPham WHERE MaSP=@ma", ("@ma", ma));
            return dt.Rows.Count == 0 ? null : Map(dt.Rows[0]);
        }

        public bool Exists(string ma)
        {
            object o = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM SanPham WHERE MaSP=@ma", ("@ma", ma));
            return Convert.ToInt32(o) > 0;
        }

        public int Insert(SanPhamDTO s)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO SanPham(MaSP, TenSP, GiaBan, SoLuongTon) VALUES(@ma, @ten, @gia, @ton)",
                ("@ma", s.MaSP), ("@ten", s.TenSP), ("@gia", s.GiaBan), ("@ton", s.SoLuongTon));
        }

        public int Update(SanPhamDTO s)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "UPDATE SanPham SET TenSP=@ten, GiaBan=@gia, SoLuongTon=@ton WHERE MaSP=@ma",
                ("@ten", s.TenSP), ("@gia", s.GiaBan), ("@ton", s.SoLuongTon), ("@ma", s.MaSP));
        }

        public int Delete(string ma)
        {
            return DatabaseHelper.ExecuteNonQuery("DELETE FROM SanPham WHERE MaSP=@ma", ("@ma", ma));
        }
    }
}

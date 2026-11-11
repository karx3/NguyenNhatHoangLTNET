using System.Data;
using RetailSales.DTO;

namespace RetailSales.DAL
{
    public class NhanVienDAL
    {
        public DataTable GetAll()
        {
            return DatabaseHelper.GetTable("SELECT MaNV, TenNV, SoDienThoai, ChucVu FROM NhanVien ORDER BY MaNV");
        }

        public DataTable Search(string keyword)
        {
            return DatabaseHelper.GetTable(
                "SELECT MaNV, TenNV, SoDienThoai, ChucVu FROM NhanVien " +
                "WHERE TenNV LIKE @k OR SoDienThoai LIKE @k ORDER BY MaNV",
                ("@k", "%" + keyword + "%"));
        }

        public List<NhanVienDTO> GetAllList()
        {
            var list = new List<NhanVienDTO>();
            foreach (DataRow r in GetAll().Rows)
            {
                list.Add(new NhanVienDTO
                {
                    MaNV = Convert.ToInt32(r["MaNV"]),
                    TenNV = r["TenNV"].ToString(),
                    SoDienThoai = DatabaseHelper.Str(r["SoDienThoai"]),
                    ChucVu = DatabaseHelper.Str(r["ChucVu"])
                });
            }
            return list;
        }

        public int Insert(NhanVienDTO n)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO NhanVien(TenNV, SoDienThoai, ChucVu) VALUES(@ten, @sdt, @cv)",
                ("@ten", n.TenNV), ("@sdt", n.SoDienThoai), ("@cv", n.ChucVu));
        }

        public int Update(NhanVienDTO n)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "UPDATE NhanVien SET TenNV=@ten, SoDienThoai=@sdt, ChucVu=@cv WHERE MaNV=@ma",
                ("@ten", n.TenNV), ("@sdt", n.SoDienThoai), ("@cv", n.ChucVu), ("@ma", n.MaNV));
        }

        public int Delete(int ma)
        {
            return DatabaseHelper.ExecuteNonQuery("DELETE FROM NhanVien WHERE MaNV=@ma", ("@ma", ma));
        }
    }
}

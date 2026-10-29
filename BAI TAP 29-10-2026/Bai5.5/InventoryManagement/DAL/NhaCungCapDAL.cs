using System.Data;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class NhaCungCapDAL
    {
        public DataTable GetAll()
        {
            return DatabaseHelper.GetTable(
                "SELECT MaNCC, TenNCC, DienThoai, DiaChi FROM NhaCungCap ORDER BY MaNCC");
        }

        // Tìm theo tên hoặc số điện thoại, dùng tham số chống SQL Injection
        public DataTable Search(string keyword)
        {
            return DatabaseHelper.GetTable(
                "SELECT MaNCC, TenNCC, DienThoai, DiaChi FROM NhaCungCap " +
                "WHERE TenNCC LIKE @k OR DienThoai LIKE @k ORDER BY MaNCC",
                ("@k", "%" + keyword + "%"));
        }

        public List<NhaCungCapDTO> GetAllList()
        {
            var list = new List<NhaCungCapDTO>();
            foreach (DataRow r in GetAll().Rows)
            {
                list.Add(new NhaCungCapDTO
                {
                    MaNCC = Convert.ToInt32(r["MaNCC"]),
                    TenNCC = r["TenNCC"].ToString()
                });
            }
            return list;
        }

        public int Insert(NhaCungCapDTO n)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO NhaCungCap(TenNCC, DienThoai, DiaChi) VALUES(@ten, @sdt, @dc)",
                ("@ten", n.TenNCC), ("@sdt", n.DienThoai), ("@dc", n.DiaChi));
        }

        public int Update(NhaCungCapDTO n)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "UPDATE NhaCungCap SET TenNCC=@ten, DienThoai=@sdt, DiaChi=@dc WHERE MaNCC=@ma",
                ("@ten", n.TenNCC), ("@sdt", n.DienThoai), ("@dc", n.DiaChi), ("@ma", n.MaNCC));
        }

        public int Delete(int ma)
        {
            return DatabaseHelper.ExecuteNonQuery("DELETE FROM NhaCungCap WHERE MaNCC=@ma", ("@ma", ma));
        }
    }
}

using System.Data;
using RetailSales.DTO;

namespace RetailSales.DAL
{
    public class KhachHangDAL
    {
        public DataTable GetAll()
        {
            return DatabaseHelper.GetTable("SELECT MaKH, TenKH, DienThoai, DiemTichLuy FROM KhachHang ORDER BY MaKH");
        }

        public DataTable Search(string keyword)
        {
            return DatabaseHelper.GetTable(
                "SELECT MaKH, TenKH, DienThoai, DiemTichLuy FROM KhachHang " +
                "WHERE TenKH LIKE @k OR DienThoai LIKE @k ORDER BY MaKH",
                ("@k", "%" + keyword + "%"));
        }

        public List<KhachHangDTO> GetAllList()
        {
            var list = new List<KhachHangDTO>();
            foreach (DataRow r in GetAll().Rows)
            {
                list.Add(new KhachHangDTO
                {
                    MaKH = Convert.ToInt32(r["MaKH"]),
                    TenKH = r["TenKH"].ToString(),
                    DienThoai = DatabaseHelper.Str(r["DienThoai"]),
                    DiemTichLuy = Convert.ToInt32(r["DiemTichLuy"])
                });
            }
            return list;
        }

        public int Insert(KhachHangDTO k)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO KhachHang(TenKH, DienThoai, DiemTichLuy) VALUES(@ten, @sdt, 0)",
                ("@ten", k.TenKH), ("@sdt", k.DienThoai));
        }

        public int Update(KhachHangDTO k)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "UPDATE KhachHang SET TenKH=@ten, DienThoai=@sdt WHERE MaKH=@ma",
                ("@ten", k.TenKH), ("@sdt", k.DienThoai), ("@ma", k.MaKH));
        }

        public int Delete(int ma)
        {
            return DatabaseHelper.ExecuteNonQuery("DELETE FROM KhachHang WHERE MaKH=@ma", ("@ma", ma));
        }
    }
}

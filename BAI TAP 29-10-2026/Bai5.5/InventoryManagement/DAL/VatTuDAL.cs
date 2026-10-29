using System.Data;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class VatTuDAL
    {
        public DataTable GetAll()
        {
            return DatabaseHelper.GetTable(
                "SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu ORDER BY MaVatTu");
        }

        // Tìm theo mã hoặc tên, dùng tham số chống SQL Injection
        public DataTable Search(string keyword)
        {
            return DatabaseHelper.GetTable(
                "SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu " +
                "WHERE MaVatTu LIKE @k OR TenVatTu LIKE @k ORDER BY MaVatTu",
                ("@k", "%" + keyword + "%"));
        }

        public List<VatTuDTO> GetAllList()
        {
            var list = new List<VatTuDTO>();
            foreach (DataRow r in GetAll().Rows)
            {
                list.Add(new VatTuDTO
                {
                    MaVatTu = r["MaVatTu"].ToString(),
                    TenVatTu = r["TenVatTu"].ToString(),
                    DVT = r["DVT"] == DBNull.Value ? "" : r["DVT"].ToString(),
                    SoLuongTon = Convert.ToDecimal(r["SoLuongTon"])
                });
            }
            return list;
        }

        public bool Exists(string ma)
        {
            object o = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM VatTu WHERE MaVatTu=@ma", ("@ma", ma));
            return Convert.ToInt32(o) > 0;
        }

        // Số lượng tồn ban đầu luôn là 0, chỉ thay đổi qua phiếu nhập/xuất kho
        public int Insert(VatTuDTO v)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO VatTu(MaVatTu, TenVatTu, DVT, SoLuongTon) VALUES(@ma, @ten, @dvt, 0)",
                ("@ma", v.MaVatTu), ("@ten", v.TenVatTu), ("@dvt", v.DVT));
        }

        public int Update(VatTuDTO v)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "UPDATE VatTu SET TenVatTu=@ten, DVT=@dvt WHERE MaVatTu=@ma",
                ("@ten", v.TenVatTu), ("@dvt", v.DVT), ("@ma", v.MaVatTu));
        }

        public int Delete(string ma)
        {
            return DatabaseHelper.ExecuteNonQuery("DELETE FROM VatTu WHERE MaVatTu=@ma", ("@ma", ma));
        }

        // Cảnh báo tồn kho: vật tư dưới ngưỡng an toàn
        public DataTable GetBelowThreshold(decimal nguong)
        {
            return DatabaseHelper.GetTable(
                "SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu WHERE SoLuongTon < @n ORDER BY SoLuongTon",
                ("@n", nguong));
        }

        // Tổng giá trị hàng tồn kho (ExecuteScalar) = tồn x đơn giá nhập bình quân
        public decimal GetTotalStockValue()
        {
            object o = DatabaseHelper.ExecuteScalar(
                "SELECT IFNULL(SUM(v.SoLuongTon * IFNULL(g.GiaTB, 0)), 0) " +
                "FROM VatTu v LEFT JOIN (" +
                "  SELECT ct.MaVatTu, SUM(ct.SoLuong * ct.DonGia) / SUM(ct.SoLuong) AS GiaTB " +
                "  FROM ChiTietPhieuKho ct JOIN PhieuKho p ON p.MaPhieu = ct.MaPhieu " +
                "  WHERE p.LoaiPhieu = 'NHAP' GROUP BY ct.MaVatTu" +
                ") g ON g.MaVatTu = v.MaVatTu");
            return Convert.ToDecimal(o);
        }
    }
}

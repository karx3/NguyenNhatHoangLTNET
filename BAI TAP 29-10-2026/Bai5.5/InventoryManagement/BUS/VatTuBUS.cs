using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class VatTuBUS
    {
        private readonly VatTuDAL dal = new VatTuDAL();

        public DataTable Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return dal.GetAll();
            return dal.Search(keyword.Trim());
        }

        public List<VatTuDTO> GetAllList()
        {
            return dal.GetAllList();
        }

        private void Validate(VatTuDTO v)
        {
            if (string.IsNullOrWhiteSpace(v.MaVatTu))
                throw new ArgumentException("Mã vật tư không được để trống.");
            if (string.IsNullOrWhiteSpace(v.TenVatTu))
                throw new ArgumentException("Tên vật tư không được để trống.");
            v.MaVatTu = v.MaVatTu.Trim();
            v.TenVatTu = v.TenVatTu.Trim();
            v.DVT = string.IsNullOrWhiteSpace(v.DVT) ? null : v.DVT.Trim();
            if (v.MaVatTu.Length > 20)
                throw new ArgumentException("Mã vật tư tối đa 20 ký tự.");
        }

        public void Add(VatTuDTO v)
        {
            Validate(v);
            if (dal.Exists(v.MaVatTu))
                throw new InvalidOperationException("Mã vật tư \"" + v.MaVatTu + "\" đã tồn tại.");
            dal.Insert(v);
        }

        public void Update(VatTuDTO v)
        {
            Validate(v);
            if (!dal.Exists(v.MaVatTu))
                throw new InvalidOperationException("Vật tư không tồn tại để sửa.");
            dal.Update(v);
        }

        public void Delete(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                throw new ArgumentException("Vui lòng chọn vật tư cần xóa.");
            dal.Delete(ma);
        }

        public DataTable GetBelowThreshold(decimal nguong)
        {
            if (nguong < 0) throw new ArgumentException("Ngưỡng không được âm.");
            return dal.GetBelowThreshold(nguong);
        }

        public decimal GetTotalStockValue()
        {
            return dal.GetTotalStockValue();
        }
    }
}

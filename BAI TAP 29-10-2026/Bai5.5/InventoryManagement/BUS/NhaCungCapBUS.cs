using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class NhaCungCapBUS
    {
        private readonly NhaCungCapDAL dal = new NhaCungCapDAL();

        public DataTable Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return dal.GetAll();
            return dal.Search(keyword.Trim());
        }

        public List<NhaCungCapDTO> GetAllList()
        {
            return dal.GetAllList();
        }

        private void Validate(NhaCungCapDTO n)
        {
            if (string.IsNullOrWhiteSpace(n.TenNCC))
                throw new ArgumentException("Tên nhà cung cấp không được để trống.");
            n.TenNCC = n.TenNCC.Trim();
            n.DienThoai = string.IsNullOrWhiteSpace(n.DienThoai) ? null : n.DienThoai.Trim();
            n.DiaChi = string.IsNullOrWhiteSpace(n.DiaChi) ? null : n.DiaChi.Trim();
            if (n.DienThoai != null && (n.DienThoai.Length > 15 || !n.DienThoai.All(char.IsDigit)))
                throw new ArgumentException("Số điện thoại chỉ gồm chữ số, tối đa 15 ký tự.");
        }

        public void Add(NhaCungCapDTO n)
        {
            Validate(n);
            dal.Insert(n);
        }

        public void Update(NhaCungCapDTO n)
        {
            if (n.MaNCC <= 0) throw new ArgumentException("Vui lòng chọn nhà cung cấp cần sửa.");
            Validate(n);
            dal.Update(n);
        }

        public void Delete(int ma)
        {
            if (ma <= 0) throw new ArgumentException("Vui lòng chọn nhà cung cấp cần xóa.");
            dal.Delete(ma);
        }
    }
}

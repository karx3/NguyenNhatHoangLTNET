using System.Data;
using RetailSales.DAL;
using RetailSales.DTO;

namespace RetailSales.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAL dal = new NhanVienDAL();

        public DataTable Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return dal.GetAll();
            return dal.Search(keyword.Trim());
        }

        public List<NhanVienDTO> GetAllList()
        {
            return dal.GetAllList();
        }

        private void Validate(NhanVienDTO n)
        {
            if (string.IsNullOrWhiteSpace(n.TenNV))
                throw new ArgumentException("Tên nhân viên không được để trống.");
            n.TenNV = n.TenNV.Trim();
            n.ChucVu = string.IsNullOrWhiteSpace(n.ChucVu) ? null : n.ChucVu.Trim();
            n.SoDienThoai = string.IsNullOrWhiteSpace(n.SoDienThoai) ? null : n.SoDienThoai.Trim();
            if (n.SoDienThoai != null && (n.SoDienThoai.Length > 15 || !n.SoDienThoai.All(char.IsDigit)))
                throw new ArgumentException("Số điện thoại chỉ gồm chữ số, tối đa 15 ký tự.");
        }

        public void Add(NhanVienDTO n)
        {
            Validate(n);
            dal.Insert(n);
        }

        public void Update(NhanVienDTO n)
        {
            if (n.MaNV <= 0) throw new ArgumentException("Vui lòng chọn nhân viên cần sửa.");
            Validate(n);
            dal.Update(n);
        }

        public void Delete(int ma)
        {
            if (ma <= 0) throw new ArgumentException("Vui lòng chọn nhân viên cần xóa.");
            dal.Delete(ma);
        }
    }
}

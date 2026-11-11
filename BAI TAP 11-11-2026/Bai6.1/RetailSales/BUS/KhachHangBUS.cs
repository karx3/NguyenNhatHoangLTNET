using System.Data;
using RetailSales.DAL;
using RetailSales.DTO;

namespace RetailSales.BUS
{
    public class KhachHangBUS
    {
        private readonly KhachHangDAL dal = new KhachHangDAL();

        public DataTable Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return dal.GetAll();
            return dal.Search(keyword.Trim());
        }

        public List<KhachHangDTO> GetAllList()
        {
            return dal.GetAllList();
        }

        private void Validate(KhachHangDTO k)
        {
            if (string.IsNullOrWhiteSpace(k.TenKH))
                throw new ArgumentException("Tên khách hàng không được để trống.");
            k.TenKH = k.TenKH.Trim();
            k.DienThoai = string.IsNullOrWhiteSpace(k.DienThoai) ? null : k.DienThoai.Trim();
            if (k.DienThoai != null && (k.DienThoai.Length > 15 || !k.DienThoai.All(char.IsDigit)))
                throw new ArgumentException("Số điện thoại chỉ gồm chữ số, tối đa 15 ký tự.");
        }

        public void Add(KhachHangDTO k)
        {
            Validate(k);
            dal.Insert(k);
        }

        public void Update(KhachHangDTO k)
        {
            if (k.MaKH <= 0) throw new ArgumentException("Vui lòng chọn khách hàng cần sửa.");
            Validate(k);
            dal.Update(k);
        }

        public void Delete(int ma)
        {
            if (ma <= 0) throw new ArgumentException("Vui lòng chọn khách hàng cần xóa.");
            dal.Delete(ma);
        }
    }
}

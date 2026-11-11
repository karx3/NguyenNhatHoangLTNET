using System.Data;
using RetailSales.DAL;
using RetailSales.DTO;

namespace RetailSales.BUS
{
    public class SanPhamBUS
    {
        private readonly SanPhamDAL dal = new SanPhamDAL();

        public DataTable Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return dal.GetAll();
            return dal.Search(keyword.Trim());
        }

        public List<SanPhamDTO> GetAllList()
        {
            return dal.GetAllList();
        }

        public SanPhamDTO GetByMa(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma)) return null;
            return dal.GetByMa(ma.Trim());
        }

        private void Validate(SanPhamDTO s)
        {
            if (string.IsNullOrWhiteSpace(s.MaSP))
                throw new ArgumentException("Mã sản phẩm (mã vạch) không được để trống.");
            if (string.IsNullOrWhiteSpace(s.TenSP))
                throw new ArgumentException("Tên sản phẩm không được để trống.");
            s.MaSP = s.MaSP.Trim();
            s.TenSP = s.TenSP.Trim();
            if (s.MaSP.Length > 20)
                throw new ArgumentException("Mã sản phẩm tối đa 20 ký tự.");
            if (s.GiaBan <= 0)
                throw new ArgumentException("Giá bán phải lớn hơn 0.");
            if (s.SoLuongTon < 0)
                throw new ArgumentException("Số lượng tồn không được âm.");
        }

        public void Add(SanPhamDTO s)
        {
            Validate(s);
            if (dal.Exists(s.MaSP))
                throw new InvalidOperationException("Mã sản phẩm \"" + s.MaSP + "\" đã tồn tại.");
            dal.Insert(s);
        }

        public void Update(SanPhamDTO s)
        {
            Validate(s);
            if (!dal.Exists(s.MaSP))
                throw new InvalidOperationException("Sản phẩm không tồn tại để sửa.");
            dal.Update(s);
        }

        public void Delete(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                throw new ArgumentException("Vui lòng chọn sản phẩm cần xóa.");
            dal.Delete(ma);
        }
    }
}

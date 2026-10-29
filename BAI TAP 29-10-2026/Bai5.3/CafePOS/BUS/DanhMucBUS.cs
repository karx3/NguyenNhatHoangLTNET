using CafePOS.DAL;
using CafePOS.DTO;

namespace CafePOS.BUS
{
    public class DanhMucBUS
    {
        private readonly DanhMucDAL dal = new DanhMucDAL();

        public List<DanhMucDTO> GetAll()
        {
            return dal.GetAll();
        }

        public void Add(string ten)
        {
            if (string.IsNullOrWhiteSpace(ten))
                throw new ArgumentException("Tên danh mục không được để trống.");
            dal.Insert(ten.Trim());
        }

        public void Update(int ma, string ten)
        {
            if (ma <= 0) throw new ArgumentException("Vui lòng chọn danh mục cần sửa.");
            if (string.IsNullOrWhiteSpace(ten))
                throw new ArgumentException("Tên danh mục không được để trống.");
            dal.Update(ma, ten.Trim());
        }

        public void Delete(int ma)
        {
            if (ma <= 0) throw new ArgumentException("Vui lòng chọn danh mục cần xóa.");
            dal.Delete(ma);
        }
    }
}

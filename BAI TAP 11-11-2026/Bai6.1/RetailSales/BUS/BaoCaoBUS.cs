using RetailSales.DAL;
using RetailSales.DTO;

namespace RetailSales.BUS
{
    public class BaoCaoBUS
    {
        private readonly HoaDonDAL dal = new HoaDonDAL();

        public decimal DoanhThuNgay(DateTime ngay)
        {
            return dal.GetDoanhThuNgay(ngay);
        }

        public decimal DoanhThuThang(int thang, int nam)
        {
            if (thang < 1 || thang > 12)
                throw new ArgumentException("Tháng phải từ 1 đến 12.");
            return dal.GetDoanhThuThang(thang, nam);
        }

        public List<TopSanPhamDTO> Top5()
        {
            return dal.GetTopSanPham(5);
        }
    }
}

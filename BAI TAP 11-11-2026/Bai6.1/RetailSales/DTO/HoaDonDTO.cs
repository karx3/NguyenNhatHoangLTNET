namespace RetailSales.DTO
{
    public class HoaDonDTO
    {
        public int MaHD { get; set; }
        public int MaNV { get; set; }
        public int? MaKH { get; set; }      // null = khách lẻ
        public decimal TongTien { get; set; }
        public int DiemCong { get; set; }
        public List<ChiTietHoaDonDTO> ChiTiet { get; set; } = new List<ChiTietHoaDonDTO>();
    }
}

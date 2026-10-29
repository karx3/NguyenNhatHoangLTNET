namespace HotelManagement.DTO
{
    // ServiceDTO: dùng để luân chuyển dữ liệu dịch vụ giữa GUI - BUS - DAL
    public class DichVuDTO
    {
        public int MaDV { get; set; }
        public string TenDV { get; set; }
        public decimal DonGia { get; set; }

        public string HienThi
        {
            get { return TenDV + " - " + DonGia.ToString("N0") + "đ"; }
        }
    }
}

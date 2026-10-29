namespace HotelManagement.DTO
{
    // Phiếu đặt phòng đang lưu trú (chưa trả phòng)
    public class PhieuDTO
    {
        public int MaPhieu { get; set; }
        public string SoPhong { get; set; }
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public DateTime NgayNhanPhong { get; set; }
        public decimal GiaTheoGio { get; set; }

        public string HienThi
        {
            get { return SoPhong + " - " + TenKH; }
        }
    }
}

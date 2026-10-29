namespace HotelManagement.DTO
{
    // Kết quả tính tiền khi trả phòng
    public class ThanhToanDTO
    {
        public DateTime ThoiGianRa { get; set; }
        public decimal SoGio { get; set; }
        public decimal TienPhong { get; set; }
        public decimal TienDichVu { get; set; }
        public decimal TongTien { get; set; }
    }
}

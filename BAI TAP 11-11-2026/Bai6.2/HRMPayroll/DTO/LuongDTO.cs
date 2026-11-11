namespace HRMPayroll.DTO
{
    // Một dòng bảng lương của nhân viên trong tháng
    public class LuongDTO
    {
        public int MaNV { get; set; }
        public string HoTen { get; set; }
        public decimal LuongCoBan { get; set; }
        public double HeSoLuong { get; set; }
        public double SoNgayCong { get; set; }
        public double SoGioOT { get; set; }
        public decimal ThucLinh { get; set; }
    }
}

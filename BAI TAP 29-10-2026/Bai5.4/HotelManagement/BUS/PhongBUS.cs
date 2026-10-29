using System.Data;
using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS
{
    public class PhongBUS
    {
        private readonly PhongDAL dal = new PhongDAL();

        public DataTable GetAllTable()
        {
            return dal.GetAllTable();
        }

        public List<PhongDTO> GetEmptyRooms()
        {
            return dal.GetByStatus("Trống");
        }

        public void SetEmpty(string soPhong)
        {
            if (string.IsNullOrWhiteSpace(soPhong))
                throw new ArgumentException("Vui lòng chọn phòng.");
            dal.UpdateStatus(soPhong, "Trống");
        }
    }
}

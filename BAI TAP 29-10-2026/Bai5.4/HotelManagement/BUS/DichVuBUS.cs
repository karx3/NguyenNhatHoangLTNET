using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS
{
    public class DichVuBUS
    {
        private readonly DichVuDAL dal = new DichVuDAL();

        public List<DichVuDTO> GetAll()
        {
            return dal.GetAll();
        }
    }
}

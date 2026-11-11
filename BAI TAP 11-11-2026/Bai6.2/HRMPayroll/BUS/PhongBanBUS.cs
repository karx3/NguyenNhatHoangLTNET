using HRMPayroll.DAL;
using HRMPayroll.DTO;

namespace HRMPayroll.BUS
{
    public class PhongBanBUS
    {
        private readonly PhongBanDAL dal = new PhongBanDAL();

        public List<PhongBanDTO> GetAll()
        {
            return dal.GetAll();
        }
    }
}

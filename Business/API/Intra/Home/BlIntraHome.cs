using DAO.DBConnection;
using DAO.Intra.EmployeeDAO;
using DTO.Intra.Home.Output;

namespace Business.API.Intra.Home
{
    public class BlIntraHome
    {
        private readonly IntraEmployeeDAO IntraEmployeeDAO;
        public BlIntraHome(XDataDatabaseSettings settings)
        {
            IntraEmployeeDAO = new(settings);
        }

        public HomeDataOutput GetHomeData() => new(IntraEmployeeDAO.EmployeesCount());
    }
}

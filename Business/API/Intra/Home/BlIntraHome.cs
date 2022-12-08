using DAO.DBConnection;
using DAO.Intra.CompanyDao;
using DAO.Intra.PersonDAO;
using DTO.Intra.Home.Output;

namespace Business.API.Intra.Home
{
    public class BlIntraHome
    {
        private readonly PersonDAO IntraPersonDAO;
        private readonly CompanyDAO IntraCompanyDAO;
        public BlIntraHome(XDataDatabaseSettings settings)
        {
            IntraPersonDAO = new(settings);
            IntraCompanyDAO = new(settings);
        }

        public HomeDataOutput GetHomeData() => new(IntraPersonDAO.PersonsCount(), IntraCompanyDAO.CompanysCount());
    }
}

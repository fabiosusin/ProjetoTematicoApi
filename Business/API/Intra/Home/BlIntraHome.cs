using DAO.DBConnection;
using DAO.Intra.PersonDAO;
using DTO.Intra.Home.Output;

namespace Business.API.Intra.Home
{
    public class BlIntraHome
    {
        private readonly IntraPersonDAO IntraPersonDAO;
        public BlIntraHome(XDataDatabaseSettings settings)
        {
            IntraPersonDAO = new(settings);
        }

        public HomeDataOutput GetHomeData() => new(IntraPersonDAO.PersonsCount());
    }
}

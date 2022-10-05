using DAO.DBConnection.MongoDB.Settings;
using DAO.DBConnection.SqlServer.Settings;

namespace DAO.DBConnection
{
    public class XDataDatabaseSettings : IXDataDatabaseSettings
    {
        public MongoDBSettings MongoDBSettings { get; set; }
        public SqlServerSettings SqlServerSettings { get; set; }
    }

    public interface IXDataDatabaseSettings
    {
        MongoDBSettings MongoDBSettings { get; set; }
        SqlServerSettings SqlServerSettings { get; set; }
    }
}

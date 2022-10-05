using DTO.Interface;

namespace DAO.DBConnection.SqlServer.Settings
{
    public class SqlServerSettings : ISqlServerSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ISqlServerSettings : IDBSettings { }
}

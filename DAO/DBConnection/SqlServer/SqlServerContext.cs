using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DAO.DBConnection.SqlServer
{
    public class SqlServerContext : DbContext
    {
        private readonly string _conn;

        public SqlServerContext(string conn) => _conn = conn;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(_conn);

        public void RunMigrations() => Database.Migrate();

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                    entry.Property("DataAlteracao").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                    entry.Property("DataAlteracao").CurrentValue = DateTime.Now;
                }
            }

            try
            {
                return base.SaveChanges();
            }
            catch 
            {
                throw;
            }
        }
    }
}

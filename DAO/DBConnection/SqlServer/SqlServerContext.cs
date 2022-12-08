using DTO.Intra.Company.Database;
using DTO.Intra.FrequencyDB.Database;
using DTO.Intra.Interview.Database;
using DTO.Intra.Person.Database;
using DTO.Intra.Situation.Database;
using DTO.Intra.User.Database;
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

        public DbSet<Frequency> Frequency { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Interview> Interview { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Situation> Situation { get; set; }
        public DbSet<AppUser> AppUser { get; set; }

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
            catch (Exception e)
            {
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder..Add(new ServidorConfig());
            //base.OnModelCreating(modelBuilder);
        }
    }
}

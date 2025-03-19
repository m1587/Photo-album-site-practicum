using Dl.Entities;
using Microsoft.EntityFrameworkCore;
namespace Dl
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<File1> Files { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
               @"Server=bg9wqnigknf88c4nwsyf-mysql.services.clever-cloud.com;Port=3306;Database=bg9wqnigknf88c4nwsyf;User=upn8gsoncwpeuxqy;Password=51I17qLyUjoi4pktQf8T",
                new MySqlServerVersion(new Version(9, 0, 0))
            );
        }
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

    }
}

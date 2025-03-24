
using Dl.Entities;
using Microsoft.EntityFrameworkCore;
namespace Dl
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<User> Users { get; set; }
        //public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Image> Images { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
               @"Server=bg9wqnigknf88c4nwsyf-mysql.services.clever-cloud.com;Port=3306;Database=bg9wqnigknf88c4nwsyf;User=upn8gsoncwpeuxqy;Password=51I17qLyUjoi4pktQf8T",
                new MySqlServerVersion(new Version(9, 0, 0))
            );
            //var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");

            //// קביעת הגדרת החיבור למסד הנתונים
            //optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 0, 0)));
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Challenge>()
        //        .HasOne(c => c.WinnerImg)
        //        .WithMany()
        //        .HasForeignKey(c => c.WinnerImgId)
        //        .OnDelete(DeleteBehavior.SetNull);

        //    modelBuilder.Entity<Challenge>()
        //        .HasOne(c => c.WinnerUser)
        //        .WithMany()
        //        .HasForeignKey(c => c.WinnerUserId)
        //        .OnDelete(DeleteBehavior.Cascade);
        //}
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

    }
}

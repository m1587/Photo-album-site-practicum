using Dl.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dl
{
    internal class DataContext : DbContext, IDataContext
    {
        public DbSet<User> Users { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=clinic_db");
        //}
        public int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}

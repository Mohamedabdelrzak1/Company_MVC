using Company.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data.Context
{
   public class CompanyDbContext : DbContext
    {

        //CLR open on configraing 

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) :base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=CompanyMvc;Trusted_Connection = True; TrustServerCertificate = True");
        //}
        public DbSet<Department> Departments { get; set; }

    }
}

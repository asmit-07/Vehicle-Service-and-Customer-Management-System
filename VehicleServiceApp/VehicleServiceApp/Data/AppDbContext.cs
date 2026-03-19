using Microsoft.EntityFrameworkCore;
using VehicleServiceApp.Models;

namespace VehicleServiceApp.Data
{
    public class AppDbContext : DbContext
        
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) 
        {
        }
        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ServiceRecord> Services { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    options.UseSqlServer(
        //        "Server=(localdb)\\MSSQLLocalDB;Database=VehicleServiceDB;Trusted_Connection=True;TrustServerCertificate=True;");
        //}
    }
}
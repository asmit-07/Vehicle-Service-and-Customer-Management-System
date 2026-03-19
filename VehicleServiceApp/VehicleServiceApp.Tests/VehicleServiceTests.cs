using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using VehicleServiceApp.Data;
using VehicleServiceApp.Models;
using VehicleServiceApp.Services;

namespace VehicleServiceApp.Tests
{
    public class VehicleServiceTests
    {
        private AppDbContext _context;
        private VehicleService _vehicleService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "VehicleTestDb")
                .Options;

            _context = new AppDbContext(options);
            _vehicleService = new VehicleService(_context);

            // Seed a customer (because Vehicle needs CustomerId)
            var customer = new Customer
            {
                Name = "Test User",
                Email = "test@gmail.com",
                Phone = "9999999999",
                IsActive = true
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        [Test]
        public void AddVehicle_ShouldAddVehicle()
        {
            var customer = _context.Customers.First();

            _vehicleService.AddVehicle(
                "UP83 BK 0303",
                "Hyundai",
                "Creta",
                2023,
                customer.CustomerId
            );

            var vehicles = _context.Vehicles.ToList();

            Assert.That(vehicles.Count, Is.EqualTo(1));
            Assert.That(vehicles[0].RegistrationNumber, Is.EqualTo("UP83 BK 0303"));
            Assert.That(vehicles[0].Brand, Is.EqualTo("Hyundai"));
        }

        [Test]
        public void GetAllVehicles_ShouldReturnVehicles()
        {
            var customer = _context.Customers.First();

            _vehicleService.AddVehicle(
                "UP83 AB 0664",
                "Toyota",
                "Fortuner",
                2024,
                customer.CustomerId
            );

            var result = _vehicleService.GetAllVehicles();

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
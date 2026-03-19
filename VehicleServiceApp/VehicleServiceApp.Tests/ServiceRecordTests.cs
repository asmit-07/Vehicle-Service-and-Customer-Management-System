using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using VehicleServiceApp.Data;
using VehicleServiceApp.Models;
using VehicleServiceApp.Services;

namespace VehicleServiceApp.Tests
{
    public class ServiceRecordTests
    {
        private AppDbContext _context;
        private ServiceRecordService _serviceRecordService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Service")
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _serviceRecordService = new ServiceRecordService(_context);
        }

        [Test]
        public void AddService_ShouldAddServiceRecord()
        {
            // Arrange

            // Add Customer
            var customer = new Customer
            {
                Name = "Asmit",
                Email = "asmit@gmail.com",
                Phone = "7900720664",
                IsActive = true
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            // Add Vehicle
            var vehicle = new Vehicle
            {
                RegistrationNumber = "UP83 BK 0303",
                Brand = "Hyundai",
                Model = "Creta",
                Year = 2023,
                CustomerId = customer.CustomerId
            };

            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            // Act
            _serviceRecordService.AddService(
                vehicle.VehicleId,
                "maintenance",
                4500
            );

            // Assert
            var services = _context.Services.ToList();

            Assert.That(services.Count, Is.EqualTo(1));
            Assert.That(services[0].ServiceType, Is.EqualTo("maintenance"));
            Assert.That(services[0].Cost, Is.EqualTo(4500));
            Assert.That(services[0].VehicleId, Is.EqualTo(vehicle.VehicleId));
            Assert.That(services[0].IsCompleted, Is.False);
        }

        [Test]
        public void AddService_ShouldThrowException_WhenCostIsZero()
        {
            // Arrange

            var customer = new Customer
            {
                Name = "Asmit",
                Email = "asmit@gmail.com",
                Phone = "7900720664",
                IsActive = true
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            var vehicle = new Vehicle
            {
                RegistrationNumber = "UP83 BK 0303",
                Brand = "Hyundai",
                Model = "Creta",
                Year = 2023,
                CustomerId = customer.CustomerId
            };

            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            // Act & Assert
            Assert.Throws<System.Exception>(() =>
                _serviceRecordService.AddService(
                    vehicle.VehicleId,
                    "maintenance",
                    0
                )
            );
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using VehicleServiceApp.Data;
using VehicleServiceApp.Services;
using VehicleServiceApp.Models;
using System.Linq;

namespace VehicleServiceApp.Tests
{
    public class CustomerServiceTests
    {
        private AppDbContext _context;
        private CustomerService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("CustomerTestDb")
                .Options;

            _context = new AppDbContext(options);
            _service = new CustomerService(_context);
        }

        [Test]
        public void AddCustomer_Should_Add_Customer()
        {
            _service.AddCustomer("Asmit", "asmit11@gmail.com", "7900720664");

            var count = _context.Customers.Count();

            Assert.That(count, Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
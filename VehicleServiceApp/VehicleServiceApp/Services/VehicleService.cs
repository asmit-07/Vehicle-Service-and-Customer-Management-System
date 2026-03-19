using System;
using System.Collections.Generic;
using System.Linq;
using VehicleServiceApp.Data;
using VehicleServiceApp.Models;

namespace VehicleServiceApp.Services
{
    public class VehicleService
    {
        private readonly AppDbContext _context;

        public VehicleService(AppDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Add Vehicle
        public void AddVehicle(string registrationNumber, string brand,
                       string model, int year, int customerId)
        {
            var customer = _context.Customers.Find(customerId);

            if (customer == null)
                throw new Exception("Customer does not exist.");

            if (!customer.IsActive)
                throw new Exception("Customer is deactivated.");

            var vehicle = new Vehicle
            {
                RegistrationNumber = registrationNumber,
                Brand = brand,
                Model = model,
                Year = year,
                CustomerId = customerId
            };

            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
        }

        // 2️⃣ Update Vehicle
        public void UpdateVehicle(int vehicleId)
        {
            var vehicle = _context.Vehicles.Find(vehicleId);

            if (vehicle == null)
                throw new Exception("Vehicle not found.");

            Console.Write("Enter New Registration Number: ");
            vehicle.RegistrationNumber = Console.ReadLine();

            Console.Write("Enter New Brand: ");
            vehicle.Brand = Console.ReadLine();

            Console.Write("Enter New Model: ");
            vehicle.Model = Console.ReadLine();

            Console.Write("Enter New Year: ");
            vehicle.Year = int.Parse(Console.ReadLine());

            _context.SaveChanges();
        }

        // 3️⃣ Delete Vehicle
        public void DeleteVehicle(int vehicleId)
        {
            var vehicle = _context.Vehicles
                                  .FirstOrDefault(v => v.VehicleId == vehicleId);

            if (vehicle == null)
                throw new Exception("Vehicle not found.");

            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
        }

        // 4️⃣ View All Vehicles
        public List<Vehicle> GetAllVehicles()
        {
            return _context.Vehicles
                           .OrderBy(v => v.VehicleId)
                           .ToList();
        }

        // 5️⃣ Search Vehicle by Registration Number (LINQ Required)
        public Vehicle? SearchByRegistration(string registrationNumber)
        {
            return _context.Vehicles
                           .FirstOrDefault(v => v.RegistrationNumber == registrationNumber);
        }

        // 6️⃣ View Vehicles by Customer (LINQ Required)
        public List<Vehicle> GetVehiclesByCustomer(int customerId)
        {
            return _context.Vehicles
                           .Where(v => v.CustomerId == customerId)
                           .ToList();
        }
    }
}
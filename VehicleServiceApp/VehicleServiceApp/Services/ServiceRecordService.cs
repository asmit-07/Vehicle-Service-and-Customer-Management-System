using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VehicleServiceApp.Data;
using VehicleServiceApp.Models;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceApp.Services
{
    public class ServiceRecordService
    {
        private readonly AppDbContext _context;

        public ServiceRecordService(AppDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Add Service Record
        public void AddService(int vehicleId, string serviceType, decimal cost)
        {
            if (cost <= 0)
                throw new Exception("Service cost must be greater than zero");

            var service = new ServiceRecord
            {
                VehicleId = vehicleId,
                ServiceType = serviceType,
                Cost = cost,
                ServiceDate = DateTime.Now,
                IsCompleted = false
            };

            _context.Services.Add(service);
            _context.SaveChanges();
        }

        // 2️⃣ Update Service Record
        public void UpdateService(int serviceId, decimal newCost, string serviceType)
        {
            var service = _context.Services
                                  .FirstOrDefault(s => s.ServiceId == serviceId);

            if (service == null)
                throw new Exception("Service not found.");

            service.Cost = newCost;
            service.ServiceType = serviceType;

            _context.SaveChanges();
        }

        // 3️⃣ Mark Service as Completed
        public void MarkServiceCompleted(int serviceId)
        {
            var service = _context.Services
                                  .FirstOrDefault(s => s.ServiceId == serviceId);

            if (service == null)
                throw new Exception("Service not found.");

            if (service.IsCompleted)
                throw new Exception("Service already completed.");

            service.IsCompleted = true;

            _context.SaveChanges();
        }

        // 4️⃣ View All Services
        public List<ServiceRecord> GetAllServices()
        {
            return _context.Services
                           .OrderBy(s => s.ServiceDate)
                           .ToList();
        }

        // 5️⃣ View Pending Services
        public List<ServiceRecord> GetPendingServices()
        {
            return _context.Services
                           .Where(s => !s.IsCompleted)
                           .ToList();
        }

        // 6️⃣ Sort Services by Cost
        public List<ServiceRecord> SortServicesByCost()
        {
            return _context.Services
                           .OrderBy(s => s.Cost)
                           .ToList();
        }

        // 7️⃣ Group Services by Vehicle
        public List<IGrouping<int, ServiceRecord>>GroupServicesByVehicle()
        {
            return _context.Services
                           .GroupBy(s => s.VehicleId)
                           .ToList();
        }

        // 8️⃣ Total Service Cost Per Customer
        public List<(int CustomerId, decimal TotalCost)> GetTotalCostPerCustomer()
        {
            return _context.Services
                .Include(s => s.Vehicle)
                .GroupBy(s => s.Vehicle!.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    TotalCost = g.Sum(s => s.Cost)
                })
                .AsEnumerable()   // <-- IMPORTANT LINE
                .Select(x => (x.CustomerId, x.TotalCost))
                .ToList();
        }
    }
}
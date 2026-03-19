using VehicleServiceApp.Data;
using VehicleServiceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceApp.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        // ADD
        public void AddCustomer(string? name, string? email, string? phone)
        {
            var customer = new Customer
            {
                Name = name,
                Email = email,
                Phone = phone,
                IsActive = true
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        // GET ALL
        public List<Customer> GetAllCustomers()
        {
            return _context.Customers
                           .OrderBy(c => c.CustomerId)
                           .ToList();
        }

        // UPDATE
        public void UpdateCustomer(Customer updatedCustomer)
        {
            var customer = _context.Customers.Find(updatedCustomer.CustomerId);

            if (customer == null)
            {
                throw new Exception("Customer not found");
            }

            customer.Name = updatedCustomer.Name;
            customer.Email = updatedCustomer.Email;
            customer.Phone = updatedCustomer.Phone;

            _context.SaveChanges();
        }

        // DELETE (Hard Delete)
        public void DeleteCustomer(int customerId)
        {
            var customer = _context.Customers
                .Include(c => c.Vehicles)
                .FirstOrDefault(c => c.CustomerId == customerId);

            // 🔴 FIRST CHECK: Exists?
            if (customer == null)
            {
                throw new Exception("Customer not found!");
            }

            // 🔴 SECOND CHECK: Has Vehicles?
            if (customer.Vehicles != null && customer.Vehicles.Any())
            {
                throw new Exception("Cannot delete customer with vehicles.");
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        // DEACTIVATE (Soft Delete)
        public void DeactivateCustomer(int customerId)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.CustomerId == customerId);

            // 🔴 CHECK 1 — Exists?
            if (customer == null)
            {
                throw new Exception("Customer not found!");
            }

            // 🔴 CHECK 2 — Already inactive?
            if (!customer.IsActive)
            {
                throw new Exception("Customer already deactivated.");
            }

            customer.IsActive = false;
            _context.SaveChanges();
        }
    }
}
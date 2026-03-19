using System.Collections.Generic;
namespace VehicleServiceApp.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;

        public List<Vehicle>? Vehicles { get; set; }
    }
}
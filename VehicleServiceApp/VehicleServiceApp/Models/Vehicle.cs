using System.Collections.Generic;

namespace VehicleServiceApp.Models
{
    public class Vehicle
    {
        
        public int VehicleId { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Model { get; set; }
        public string? Brand { get; set; }
        public int Year { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public List<ServiceRecord>? Services { get; set; }
    }
}
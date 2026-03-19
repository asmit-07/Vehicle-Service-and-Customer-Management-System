using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleServiceApp.Models
{
    public class ServiceRecord
    {
        [Key]
        public int ServiceId { get; set; }
        public int VehicleId { get; set; }
        public DateTime ServiceDate { get; set; }
        public string? ServiceType { get; set; }
        public decimal Cost { get; set; }
        public bool IsCompleted { get; set; } = false;
        public Vehicle? Vehicle { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; } = null!;
        public int YearOfManufacture { get; set; }
        public decimal WeightKg { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}

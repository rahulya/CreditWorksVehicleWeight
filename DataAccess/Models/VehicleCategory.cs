using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class VehicleCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal MinimumWeightKg { get; set; }
        public decimal? MaximumWeightKg { get; set; }
        public string Icon { get; set; } = string.Empty;
    }
}

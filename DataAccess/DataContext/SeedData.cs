using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataContext
{
    public static class SeedData
    {
        public static async Task InitializeAsync(AppDbContext db)
        {
            if (!await db.Manufacturers.AnyAsync())
            {
                db.Manufacturers.AddRange(new[] { "Mazda", "Mercedes", "Honda", "Ferrari", "Toyota" }
                    .Select(name => new Manufacturer { Name = name }));
            }
            if (!await db.VehicleCategories.AnyAsync())
            {
                db.VehicleCategories.AddRange(
                    new VehicleCategory { Name = "Light", MinimumWeightKg = 0, MaximumWeightKg = 500, Icon = "🛵" },
                    new VehicleCategory { Name = "Medium", MinimumWeightKg = 500, MaximumWeightKg = 2500, Icon = "🚙" },
                    new VehicleCategory { Name = "Heavy", MinimumWeightKg = 2500, MaximumWeightKg = null, Icon = "🚚" });
            }
            await db.SaveChangesAsync();
        }
    }
}

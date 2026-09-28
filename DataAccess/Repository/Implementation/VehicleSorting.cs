using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Implementation
{
    public static class VehicleSorting
    {
        public static IQueryable<Vehicle> Apply(IQueryable<Vehicle> query, string? sortBy, string? direction)
        {
            var descending = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase);
            return (sortBy?.ToLowerInvariant(), descending) switch
            {
                ("manufacturer", false) => query.OrderBy(x => x.Manufacturer.Name).ThenBy(x => x.OwnerName),
                ("manufacturer", true) => query.OrderByDescending(x => x.Manufacturer.Name).ThenBy(x => x.OwnerName),
                ("year", false) => query.OrderBy(x => x.YearOfManufacture).ThenBy(x => x.OwnerName),
                ("year", true) => query.OrderByDescending(x => x.YearOfManufacture).ThenBy(x => x.OwnerName),
                ("weight", false) => query.OrderBy(x => x.WeightKg).ThenBy(x => x.OwnerName),
                ("weight", true) => query.OrderByDescending(x => x.WeightKg).ThenBy(x => x.OwnerName),
                ("owner", true) => query.OrderByDescending(x => x.OwnerName),
                _ => query.OrderBy(x => x.OwnerName)
            };
        }
    }
}

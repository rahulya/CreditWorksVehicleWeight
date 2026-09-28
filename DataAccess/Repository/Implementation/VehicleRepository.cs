using DataAccess.DataContext;
using DataAccess.Models;
using DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace DataAccess.Repository.Implementation
{
    public sealed class VehicleRepository(AppDbContext context) : EfRepository<Vehicle>(context), IVehicleRepository
    {
        public async Task<IReadOnlyList<Vehicle>> GetSortedAsync(string sortBy, string direction, CancellationToken cancellationToken = default)
        {
            IQueryable<Vehicle> query = Entities.AsNoTracking().Include(x => x.Manufacturer);
            query = VehicleSorting.Apply(query, sortBy, direction);
            return await query.ToListAsync(cancellationToken);
        }

        public Task<bool> ManufacturerExistsAsync(int manufacturerId, CancellationToken cancellationToken = default) =>
            Context.Manufacturers.AnyAsync(x => x.Id == manufacturerId, cancellationToken);
    }
}

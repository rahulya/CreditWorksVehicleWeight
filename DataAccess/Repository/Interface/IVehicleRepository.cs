using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Interface
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<IReadOnlyList<Vehicle>> GetSortedAsync(string sortBy, string direction, CancellationToken cancellationToken = default);
        Task<bool> ManufacturerExistsAsync(int manufacturerId, CancellationToken cancellationToken = default);
    }
}

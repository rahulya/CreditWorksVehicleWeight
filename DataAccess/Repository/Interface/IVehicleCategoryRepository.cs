using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Interface
{
    public interface IVehicleCategoryRepository : IRepository<VehicleCategory>
    {
        Task ReplaceConfigurationAsync(IReadOnlyCollection<VehicleCategory> categories, CancellationToken cancellationToken = default);
    }
}

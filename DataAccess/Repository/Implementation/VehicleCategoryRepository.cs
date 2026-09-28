using DataAccess.DataContext;
using DataAccess.Models;
using DataAccess.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Repository.Implementation
{
    public sealed class VehicleCategoryRepository(AppDbContext context) : EfRepository<VehicleCategory>(context), IVehicleCategoryRepository
    {
        /// <summary>
        /// Replaces all existing vehicle categories with the provided collection in a single transaction.
        /// </summary>
        /// <param name="categories">The collection of vehicle categories to replace existing ones with.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ReplaceConfigurationAsync(IReadOnlyCollection<VehicleCategory> categories, CancellationToken cancellationToken = default)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken);
            var existing = await Entities.ToListAsync(cancellationToken);
            Entities.RemoveRange(existing);
            await Entities.AddRangeAsync(categories, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
    }
}

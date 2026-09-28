using DataAccess.DataContext;
using DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Implementation
{
    public class EfRepository<TEntity>(AppDbContext context) : IRepository<TEntity> where TEntity : class
    {
        protected AppDbContext Context { get; } = context;
        protected DbSet<TEntity> Entities => Context.Set<TEntity>();

        public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await Entities.AsNoTracking().ToListAsync(cancellationToken);

        public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
            await Entities.FindAsync([id], cancellationToken);

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Entities.AddAsync(entity, cancellationToken);
        }

        public virtual void Update(TEntity entity) => Entities.Update(entity);

        public virtual void Delete(TEntity entity) => Entities.Remove(entity);

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            Context.SaveChangesAsync(cancellationToken);
    }
}

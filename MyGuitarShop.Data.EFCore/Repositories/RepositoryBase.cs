using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Data.EFCore.Context;

namespace MyGuitarShop.Data.EFCore.Repositories
{
    public abstract class RepositoryBase<TEntity>(
        MyGuitarShopContext dbContext) 
        : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public virtual async Task<TEntity?> FindByIdAsync(int id) =>
            await _dbSet.FindAsync(id);

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);

            return await dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateAsync(int id, TEntity dto)
        {
            var existingEntity = await _dbSet.FindAsync(id);

            if (existingEntity == null)
                return 0;

            _dbSet.Entry(existingEntity).CurrentValues.SetValues(dto);

            return await dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
                return 0;

            _dbSet.Remove(entity);

            return await dbContext.SaveChangesAsync();
        }
    }
}


using Authentication_Practice.Data;
using Authentication_Practice.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Authentication_Practice.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            this._dbSet = _dbContext.Set<TEntity>();
        }

        public async ValueTask DisposeAsync()
        {
            if (_dbContext != null)
            {
                await _dbContext.DisposeAsync();
            }
        }

        public async Task<List<TDto>> GetAllAsync<TDto>(
            Expression<Func<TEntity, TDto>> selector
        )
        {
            return await _dbSet
                .AsNoTracking()
                .Select(selector)
                .ToListAsync();
        }

        public IQueryable<TEntity> GetQuery()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<TEntity?> GetEntityById(Guid Id)
        {
            return await _dbSet.SingleOrDefaultAsync(d => d.Id == Id);
        }

        public async Task AddEntity(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.LastUpdateDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeEntities(List<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                entity.CreateDate = DateTime.Now;
                entity.LastUpdateDate = DateTime.Now;
                await _dbSet.AddAsync(entity);
            }
        }
        public void EditEntity(TEntity entity)
        {
            entity.LastUpdateDate = DateTime.Now;
            _dbSet.Update(entity);
        }

        //public void DeleteEntity(TEntity entity)
        //{
        //    item.IsDeleted = true;
        //    EditEntity(item);
        //}

        //public void DeleteRangeEntities(List<TEntity> entities)
        //{
        //    foreach(var item in entities)
        //    {
        //        item.IsDeleted = true;
        //        EditEntity(item);
        //    }
        //}

        public async Task DeletePermanent(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeletePermanentEntities(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

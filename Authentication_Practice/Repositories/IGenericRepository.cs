using Authentication_Practice.Models;
using System.Linq.Expressions;

namespace Authentication_Practice.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TDto>> GetAllAsync<TDto>(
            Expression<Func<TEntity, TDto>> selector
        );

        IQueryable<TEntity> GetQuery();
        Task<TEntity?> GetEntityById(Guid id);
        Task AddEntity(TEntity entity);
        Task AddRangeEntities(List<TEntity> entities);
        void EditEntity(TEntity entity);
        //void DeleteEntity(TEntity entity);
        //Task DeleteRangeEntities(List<TEntity> entities);
        void DeletePermanentEntities(List<TEntity> entities);
        Task DeletePermanent(TEntity entity);
        Task SaveAsync();
    }
}

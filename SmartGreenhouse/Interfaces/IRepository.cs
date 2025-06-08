using System.Linq.Expressions;

namespace SmartGreenhouse.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           params string[] includeProperties);

        TEntity GetById(object id);
        void Create(TEntity entity);
        void Update(TEntity entityToUpdate);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Save();
        Task AddAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task SaveAsync();
    }
}

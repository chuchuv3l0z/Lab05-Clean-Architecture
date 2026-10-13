using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab05.Application.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        
        Task AddAsync(TEntity entity);
        
        Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
        
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}

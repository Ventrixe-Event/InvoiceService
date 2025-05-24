using Persistence.Models;

namespace Persistence.Interfaces;

public interface IBaseRepository<TEntity>
    where TEntity : class
{
    Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync();
    Task<RepositoryResult<TEntity>> GetByIdAsync(Guid id);
    Task<RepositoryResult<TEntity>> CreateAsync(TEntity entity);
    Task<RepositoryResult<TEntity>> UpdateAsync(TEntity entity);
    Task<RepositoryResult> DeleteAsync(Guid id);
}

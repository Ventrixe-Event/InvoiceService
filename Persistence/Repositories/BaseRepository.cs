using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Interfaces;
using Persistence.Models;

namespace Persistence.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    protected readonly DataContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync()
    {
        try
        {
            var entities = await _dbSet.ToListAsync();
            return RepositoryResult<IEnumerable<TEntity>>.Success(entities);
        }
        catch (Exception ex)
        {
            return RepositoryResult<IEnumerable<TEntity>>.Failure(
                $"Error retrieving entities: {ex.Message}"
            );
        }
    }

    public virtual async Task<RepositoryResult<TEntity>> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return RepositoryResult<TEntity>.Failure("Entity not found");
            }
            return RepositoryResult<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            return RepositoryResult<TEntity>.Failure($"Error retrieving entity: {ex.Message}");
        }
    }

    public virtual async Task<RepositoryResult<TEntity>> CreateAsync(TEntity entity)
    {
        try
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return RepositoryResult<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            return RepositoryResult<TEntity>.Failure($"Error creating entity: {ex.Message}");
        }
    }

    public virtual async Task<RepositoryResult<TEntity>> UpdateAsync(TEntity entity)
    {
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return RepositoryResult<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            return RepositoryResult<TEntity>.Failure($"Error updating entity: {ex.Message}");
        }
    }

    public virtual async Task<RepositoryResult> DeleteAsync(Guid id)
    {
        try
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return RepositoryResult.Failure("Entity not found");
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return RepositoryResult.Success();
        }
        catch (Exception ex)
        {
            return RepositoryResult.Failure($"Error deleting entity: {ex.Message}");
        }
    }
}

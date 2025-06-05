/**
 * InvoiceService Base Repository Implementation - Generic Data Access Layer
 *
 * Purpose: Abstract base repository implementation providing common CRUD operations for all entities
 * Features:
 * - Generic Entity Framework Core operations with type safety
 * - Result pattern implementation for consistent error handling
 * - Comprehensive exception handling with detailed error messages
 * - Virtual methods allowing derived repositories to override behavior
 * - Database context abstraction for testability and flexibility
 *
 * Architecture: Part of the Persistence layer in clean architecture
 * - Implements IBaseRepository<TEntity> interface contract
 * - Provides foundation for domain-specific repository implementations
 * - Encapsulates Entity Framework Core operations and error handling
 * - Supports dependency injection and unit testing scenarios
 *
 * Error Handling Strategy:
 * - All database exceptions are caught and converted to RepositoryResult failures
 * - Detailed error messages preserved for logging and debugging
 * - Consistent error response format across all operations
 * - No exceptions propagate to calling layers
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Interfaces;
using Persistence.Models;

namespace Persistence.Repositories;

/// <summary>
/// Abstract base repository implementation providing common CRUD operations for Entity Framework entities.
/// Implements the repository pattern with result-based error handling and comprehensive exception management.
/// Serves as foundation for domain-specific repositories while ensuring consistent data access patterns.
/// All operations return RepositoryResult for uniform error handling across the application.
/// </summary>
/// <typeparam name="TEntity">The entity type that this repository manages, must be a reference type</typeparam>
public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Entity Framework Core database context for data access operations.
    /// Provides access to the database and change tracking functionality.
    /// Protected to allow derived repositories to access context directly if needed.
    /// </summary>
    protected readonly DataContext _context;

    /// <summary>
    /// Entity Framework Core DbSet for the specific entity type.
    /// Provides strongly-typed access to entity operations and LINQ queries.
    /// Protected to allow derived repositories to perform custom queries.
    /// </summary>
    protected readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Initializes a new instance of the BaseRepository class with the specified database context.
    /// Sets up the DbSet for the generic entity type to enable type-safe database operations.
    /// </summary>
    /// <param name="context">The Entity Framework Core database context for data access</param>
    protected BaseRepository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    /// <summary>
    /// Retrieves all entities of the specified type from the database.
    /// Executes a query to fetch all records and returns them as a collection.
    /// Virtual method allows derived repositories to override with specific behavior.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with an enumerable collection of entities.</returns>
    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync()
    {
        try
        {
            // Execute async query to retrieve all entities from database
            var entities = await _dbSet.ToListAsync();
            return RepositoryResult<IEnumerable<TEntity>>.Success(entities);
        }
        catch (Exception ex)
        {
            // Convert exception to result failure with detailed error message
            return RepositoryResult<IEnumerable<TEntity>>.Failure(
                $"Error retrieving entities: {ex.Message}"
            );
        }
    }

    /// <summary>
    /// Retrieves a specific entity by its unique identifier from the database.
    /// Uses Entity Framework's FindAsync for optimal performance with primary key lookups.
    /// Returns failure result if entity is not found or database error occurs.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with the entity if found.</returns>
    public virtual async Task<RepositoryResult<TEntity>> GetByIdAsync(Guid id)
    {
        try
        {
            // Use FindAsync for efficient primary key lookup
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                // Return failure result for not found entities
                return RepositoryResult<TEntity>.Failure("Entity not found");
            }
            return RepositoryResult<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            // Convert exception to result failure with detailed error message
            return RepositoryResult<TEntity>.Failure($"Error retrieving entity: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new entity in the database and persists the changes.
    /// Adds the entity to the change tracker and saves changes to commit the transaction.
    /// Returns the created entity with any generated values populated.
    /// </summary>
    /// <param name="entity">The entity to create in the database</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with the created entity.</returns>
    public virtual async Task<RepositoryResult<TEntity>> CreateAsync(TEntity entity)
    {
        try
        {
            // Add entity to change tracker for creation
            _dbSet.Add(entity);

            // Persist changes to database
            await _context.SaveChangesAsync();

            return RepositoryResult<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            // Convert exception to result failure with detailed error message
            return RepositoryResult<TEntity>.Failure($"Error creating entity: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing entity in the database and persists the changes.
    /// Marks the entity as modified in the change tracker and saves changes.
    /// Assumes the entity is already being tracked or contains all necessary data.
    /// </summary>
    /// <param name="entity">The entity with updated data to persist to the database</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with the updated entity.</returns>
    public virtual async Task<RepositoryResult<TEntity>> UpdateAsync(TEntity entity)
    {
        try
        {
            // Mark entity as modified in change tracker
            _dbSet.Update(entity);

            // Persist changes to database
            await _context.SaveChangesAsync();

            return RepositoryResult<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            // Convert exception to result failure with detailed error message
            return RepositoryResult<TEntity>.Failure($"Error updating entity: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes an entity from the database by its unique identifier.
    /// First retrieves the entity to ensure it exists, then removes it and persists changes.
    /// Returns failure if entity is not found or database error occurs during deletion.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult indicating success or failure.</returns>
    public virtual async Task<RepositoryResult> DeleteAsync(Guid id)
    {
        try
        {
            // First find the entity to ensure it exists
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                // Return failure result for not found entities
                return RepositoryResult.Failure("Entity not found");
            }

            // Remove entity from change tracker
            _dbSet.Remove(entity);

            // Persist changes to database
            await _context.SaveChangesAsync();

            return RepositoryResult.Success();
        }
        catch (Exception ex)
        {
            // Convert exception to result failure with detailed error message
            return RepositoryResult.Failure($"Error deleting entity: {ex.Message}");
        }
    }
}

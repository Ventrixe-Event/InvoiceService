/**
 * InvoiceService Base Repository Interface - Generic Data Access Contract
 *
 * Purpose: Generic repository interface defining common CRUD operations for all entities
 * Features:
 * - Generic type constraint for entity classes
 * - Standardized CRUD operations with result pattern
 * - Asynchronous operations for optimal performance
 * - Consistent error handling through RepositoryResult
 * - Foundation for specific repository implementations
 *
 * Architecture: Part of the Persistence layer in clean architecture
 * - Implements repository pattern for data access abstraction
 * - Provides consistent interface for Entity Framework operations
 * - Supports dependency inversion and testability
 * - Enables result pattern for error handling consistency
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using Persistence.Models;

namespace Persistence.Interfaces;

/// <summary>
/// Generic repository interface defining common CRUD operations for entity data access.
/// Provides a consistent contract for all repository implementations with result pattern integration.
/// Uses generic type constraints to ensure type safety while maintaining code reusability.
/// All operations return RepositoryResult for consistent error handling and success indication.
/// </summary>
/// <typeparam name="TEntity">The entity type that this repository manages, must be a reference type</typeparam>
public interface IBaseRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Retrieves all entities of the specified type from the data store.
    /// Returns a comprehensive collection of entities wrapped in a result object
    /// for consistent error handling and success indication.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with an enumerable collection of entities.</returns>
    Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync();

    /// <summary>
    /// Retrieves a specific entity by its unique identifier.
    /// Searches the data store for an entity matching the provided ID
    /// and returns the result wrapped in a RepositoryResult for error handling.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with the entity if found.</returns>
    Task<RepositoryResult<TEntity>> GetByIdAsync(Guid id);

    /// <summary>
    /// Creates a new entity in the data store.
    /// Adds the provided entity to the data store and persists the changes.
    /// Returns the created entity with any generated values (like ID) populated.
    /// </summary>
    /// <param name="entity">The entity to create in the data store</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with the created entity.</returns>
    Task<RepositoryResult<TEntity>> CreateAsync(TEntity entity);

    /// <summary>
    /// Updates an existing entity in the data store.
    /// Modifies the specified entity with the provided data and persists the changes.
    /// Returns the updated entity reflecting the current state after modification.
    /// </summary>
    /// <param name="entity">The entity with updated data to persist to the data store</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with the updated entity.</returns>
    Task<RepositoryResult<TEntity>> UpdateAsync(TEntity entity);

    /// <summary>
    /// Deletes an entity from the data store by its unique identifier.
    /// Removes the entity matching the provided ID and persists the changes.
    /// Returns a result indicating the success or failure of the deletion operation.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult indicating success or failure.</returns>
    Task<RepositoryResult> DeleteAsync(Guid id);
}

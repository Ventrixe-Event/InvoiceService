/**
 * InvoiceService Repository Result Models - Result Pattern Implementation
 *
 * Purpose: Result pattern implementation for consistent error handling in the data access layer
 * Features:
 * - Generic and non-generic result types for different scenarios
 * - Success and failure factory methods for easy result creation
 * - Consistent error handling across all repository operations
 * - Type-safe data encapsulation with nullable data property
 * - Standardized result structure for business logic consumption
 *
 * Architecture: Part of the Persistence layer in clean architecture
 * - Implements result pattern for functional error handling
 * - Provides consistent interface between persistence and application layers
 * - Eliminates exception-based error handling in favor of explicit results
 * - Supports both data-returning and operation-only scenarios
 *
 * Usage Patterns:
 * - RepositoryResult<T>: For operations returning data
 * - RepositoryResult: For operations with success/failure only
 * - Factory methods: For easy result creation and consistency
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

namespace Persistence.Models;

/// <summary>
/// Generic result wrapper for repository operations that return data.
/// Encapsulates the operation outcome, error information, and result data
/// in a consistent structure for reliable error handling and data access.
/// Provides factory methods for easy creation of success and failure results.
/// </summary>
/// <typeparam name="T">The type of data returned by the repository operation</typeparam>
public class RepositoryResult<T>
{
    /// <summary>
    /// Gets or sets a value indicating whether the repository operation completed successfully.
    /// True indicates successful operation with valid data; false indicates failure with error information.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the error message describing what went wrong during the operation.
    /// Populated only when IsSuccess is false; null or empty when operation succeeds.
    /// Provides detailed information for logging, debugging, and user feedback.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the data returned by the successful repository operation.
    /// Contains the requested data when IsSuccess is true; null when operation fails.
    /// Type corresponds to the generic parameter specified when creating the result.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Creates a successful repository result with the specified data.
    /// Factory method that ensures consistent result creation for successful operations
    /// and automatically sets the IsSuccess flag to true.
    /// </summary>
    /// <param name="data">The data to include in the successful result</param>
    /// <returns>A RepositoryResult indicating success with the provided data</returns>
    public static RepositoryResult<T> Success(T data)
    {
        return new RepositoryResult<T> { IsSuccess = true, Data = data };
    }

    /// <summary>
    /// Creates a failed repository result with the specified error message.
    /// Factory method that ensures consistent result creation for failed operations
    /// and automatically sets the IsSuccess flag to false.
    /// </summary>
    /// <param name="errorMessage">The error message describing the failure</param>
    /// <returns>A RepositoryResult indicating failure with the provided error message</returns>
    public static RepositoryResult<T> Failure(string errorMessage)
    {
        return new RepositoryResult<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}

/// <summary>
/// Non-generic result wrapper for repository operations that don't return data.
/// Encapsulates the operation outcome and error information for operations like
/// delete, update status changes, or other actions where only success/failure matters.
/// Provides factory methods for easy creation of success and failure results.
/// </summary>
public class RepositoryResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the repository operation completed successfully.
    /// True indicates successful operation completion; false indicates failure with error information.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the error message describing what went wrong during the operation.
    /// Populated only when IsSuccess is false; null or empty when operation succeeds.
    /// Provides detailed information for logging, debugging, and user feedback.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful repository result for operations that don't return data.
    /// Factory method that ensures consistent result creation for successful operations
    /// and automatically sets the IsSuccess flag to true.
    /// </summary>
    /// <returns>A RepositoryResult indicating successful operation completion</returns>
    public static RepositoryResult Success()
    {
        return new RepositoryResult { IsSuccess = true };
    }

    /// <summary>
    /// Creates a failed repository result with the specified error message.
    /// Factory method that ensures consistent result creation for failed operations
    /// and automatically sets the IsSuccess flag to false.
    /// </summary>
    /// <param name="errorMessage">The error message describing the failure</param>
    /// <returns>A RepositoryResult indicating failure with the provided error message</returns>
    public static RepositoryResult Failure(string errorMessage)
    {
        return new RepositoryResult { IsSuccess = false, ErrorMessage = errorMessage };
    }
}

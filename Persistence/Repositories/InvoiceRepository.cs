/**
 * InvoiceService Invoice Repository Implementation - Specialized Data Access Layer
 *
 * Purpose: Concrete implementation of invoice-specific data access operations extending base repository
 * Features:
 * - Inherits standard CRUD operations from BaseRepository<InvoiceEntity>
 * - Specialized LINQ queries for invoice business requirements
 * - Overdue invoice calculation with business rule enforcement
 * - Chronological ordering for optimal user experience
 * - Comprehensive error handling with detailed error messages
 * - Result pattern implementation for consistent error handling
 *
 * Architecture: Part of the Persistence layer in clean architecture
 * - Extends BaseRepository<InvoiceEntity> for common operations
 * - Implements IInvoiceRepository interface for domain-specific operations
 * - Encapsulates Entity Framework Core queries and business logic
 * - Provides optimized data access patterns for invoice management
 *
 * Query Optimizations:
 * - Chronological ordering by CreatedAt for user-friendly display
 * - Overdue calculation excludes paid and cancelled invoices
 * - Efficient filtering using indexed columns (EventId, UserId, Status)
 * - Proper exception handling prevents database errors from propagating
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Entities;
using Persistence.Interfaces;
using Persistence.Models;

namespace Persistence.Repositories;

/// <summary>
/// Concrete implementation of invoice-specific data access operations.
/// Extends the base repository pattern with specialized queries for invoice business requirements.
/// Provides optimized LINQ queries with proper ordering and filtering for comprehensive invoice management.
/// All operations maintain the result pattern for consistent error handling and success indication.
/// </summary>
public class InvoiceRepository : BaseRepository<InvoiceEntity>, IInvoiceRepository
{
    /// <summary>
    /// Initializes a new instance of the InvoiceRepository class with the specified database context.
    /// Configures the repository with Entity Framework Core context for invoice data access operations.
    /// </summary>
    /// <param name="context">The Entity Framework Core database context for invoice data access</param>
    public InvoiceRepository(DataContext context)
        : base(context) { }

    /// <summary>
    /// Retrieves all invoices associated with a specific event, ordered chronologically.
    /// Filters invoices by event ID and sorts by creation date (newest first) for optimal user experience.
    /// Essential for event-specific financial reporting and revenue analysis operations.
    /// </summary>
    /// <param name="eventId">The unique identifier of the event to retrieve invoices for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with an enumerable collection of event-specific invoices.</returns>
    public async Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByEventIdAsync(Guid eventId)
    {
        try
        {
            // Execute LINQ query with filtering and chronological ordering
            var invoices = await _dbSet
                .Where(i => i.EventId == eventId) // Filter by event ID
                .OrderByDescending(i => i.CreatedAt) // Order by newest first
                .ToListAsync();

            return RepositoryResult<IEnumerable<InvoiceEntity>>.Success(invoices);
        }
        catch (Exception ex)
        {
            // Convert exception to result failure with specific error context
            return RepositoryResult<IEnumerable<InvoiceEntity>>.Failure(
                $"Error retrieving invoices by event ID: {ex.Message}"
            );
        }
    }

    /// <summary>
    /// Retrieves all invoices associated with a specific user, ordered chronologically.
    /// Filters invoices by user ID and sorts by creation date (newest first) for account management.
    /// Critical for user-specific billing history and financial tracking operations.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve invoices for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with an enumerable collection of user-specific invoices.</returns>
    public async Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByUserIdAsync(Guid userId)
    {
        try
        {
            // Execute LINQ query with filtering and chronological ordering
            var invoices = await _dbSet
                .Where(i => i.UserId == userId) // Filter by user ID
                .OrderByDescending(i => i.CreatedAt) // Order by newest first
                .ToListAsync();

            return RepositoryResult<IEnumerable<InvoiceEntity>>.Success(invoices);
        }
        catch (Exception ex)
        {
            // Convert exception to result failure with specific error context
            return RepositoryResult<IEnumerable<InvoiceEntity>>.Failure(
                $"Error retrieving invoices by user ID: {ex.Message}"
            );
        }
    }

    /// <summary>
    /// Retrieves all invoices with a specific status, ordered chronologically.
    /// Filters invoices by status enum and sorts by creation date for workflow management.
    /// Essential for status-based reporting and business process automation.
    /// </summary>
    /// <param name="status">The invoice status to filter by (Draft, Sent, Paid, Overdue, Cancelled)</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with an enumerable collection of status-filtered invoices.</returns>
    public async Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByStatusAsync(
        InvoiceStatus status
    )
    {
        try
        {
            // Execute LINQ query with status filtering and chronological ordering
            var invoices = await _dbSet
                .Where(i => i.Status == status) // Filter by status enum
                .OrderByDescending(i => i.CreatedAt) // Order by newest first
                .ToListAsync();

            return RepositoryResult<IEnumerable<InvoiceEntity>>.Success(invoices);
        }
        catch (Exception ex)
        {
            // Convert exception to result failure with specific error context
            return RepositoryResult<IEnumerable<InvoiceEntity>>.Failure(
                $"Error retrieving invoices by status: {ex.Message}"
            );
        }
    }

    /// <summary>
    /// Retrieves all overdue invoices using business rule logic for collections processes.
    /// Identifies invoices past due date that are not paid or cancelled, ordered by due date.
    /// Critical for automated collections, reminder systems, and financial reporting.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with an enumerable collection of overdue invoices.</returns>
    public async Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetOverdueInvoicesAsync()
    {
        try
        {
            // Use UTC date for consistent timezone handling
            var today = DateTime.UtcNow.Date;

            // Execute complex LINQ query with business rule logic
            var invoices = await _dbSet
                .Where(i =>
                    i.DueDate < today // Past due date
                    && i.Status != InvoiceStatus.Paid // Not already paid
                    && i.Status != InvoiceStatus.Cancelled // Not cancelled
                )
                .OrderBy(i => i.DueDate) // Order by oldest due date first
                .ToListAsync();

            return RepositoryResult<IEnumerable<InvoiceEntity>>.Success(invoices);
        }
        catch (Exception ex)
        {
            // Convert exception to result failure with specific error context
            return RepositoryResult<IEnumerable<InvoiceEntity>>.Failure(
                $"Error retrieving overdue invoices: {ex.Message}"
            );
        }
    }

    /// <summary>
    /// Retrieves a specific invoice by its human-readable invoice number.
    /// Provides customer service lookup functionality using business identifier instead of GUID.
    /// Returns failure result if invoice number is not found in the system.
    /// </summary>
    /// <param name="invoiceNumber">The invoice number to search for (e.g., "INV1981", "INV2001")</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with the invoice if found.</returns>
    public async Task<RepositoryResult<InvoiceEntity>> GetByInvoiceNumberAsync(string invoiceNumber)
    {
        try
        {
            // Execute LINQ query to find invoice by unique invoice number
            var invoice = await _dbSet.FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);

            if (invoice == null)
            {
                // Return failure result for not found invoice numbers
                return RepositoryResult<InvoiceEntity>.Failure("Invoice not found");
            }

            return RepositoryResult<InvoiceEntity>.Success(invoice);
        }
        catch (Exception ex)
        {
            // Convert exception to result failure with specific error context
            return RepositoryResult<InvoiceEntity>.Failure(
                $"Error retrieving invoice by number: {ex.Message}"
            );
        }
    }

    /// <summary>
    /// Overrides the base GetAllAsync method to provide chronological ordering for invoices.
    /// Retrieves all invoices sorted by creation date (newest first) for optimal user experience.
    /// Provides consistent ordering across all invoice list operations in the application.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with an enumerable collection of chronologically ordered invoices.</returns>
    public override async Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetAllAsync()
    {
        try
        {
            // Execute query with chronological ordering override
            var invoices = await _dbSet.OrderByDescending(i => i.CreatedAt).ToListAsync();

            return RepositoryResult<IEnumerable<InvoiceEntity>>.Success(invoices);
        }
        catch (Exception ex)
        {
            // Convert exception to result failure with specific error context
            return RepositoryResult<IEnumerable<InvoiceEntity>>.Failure(
                $"Error retrieving all invoices: {ex.Message}"
            );
        }
    }
}

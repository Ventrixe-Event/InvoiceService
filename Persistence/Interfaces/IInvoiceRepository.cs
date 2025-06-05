/**
 * InvoiceService Invoice Repository Interface - Invoice-Specific Data Access Contract
 *
 * Purpose: Specialized repository interface for invoice entity operations extending base repository
 * Features:
 * - Inherits standard CRUD operations from IBaseRepository
 * - Event-specific invoice retrieval for financial reporting
 * - User-specific invoice queries for account management
 * - Status-based filtering for workflow management
 * - Overdue invoice identification for collections
 * - Invoice number lookup for customer service
 *
 * Architecture: Part of the Persistence layer in clean architecture
 * - Extends generic repository pattern with domain-specific operations
 * - Provides specialized queries for invoice business requirements
 * - Maintains consistent result pattern for error handling
 * - Supports complex filtering and reporting operations
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using Persistence.Entities;
using Persistence.Models;

namespace Persistence.Interfaces;

/// <summary>
/// Specialized repository interface for invoice entity operations.
/// Extends the base repository pattern with invoice-specific query methods
/// for comprehensive financial data access and business intelligence support.
/// All operations maintain the result pattern for consistent error handling.
/// </summary>
public interface IInvoiceRepository : IBaseRepository<InvoiceEntity>
{
    /// <summary>
    /// Retrieves all invoices associated with a specific event.
    /// Filters invoices by event ID for event-specific financial analysis,
    /// revenue reporting, and cost allocation operations.
    /// </summary>
    /// <param name="eventId">The unique identifier of the event to retrieve invoices for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with an enumerable collection of event-specific invoices.</returns>
    Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByEventIdAsync(Guid eventId);

    /// <summary>
    /// Retrieves all invoices associated with a specific user.
    /// Filters invoices by user ID for customer account management,
    /// billing history review, and user-specific financial tracking.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve invoices for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with an enumerable collection of user-specific invoices.</returns>
    Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves all invoices with a specific status.
    /// Filters invoices by status enum for workflow management,
    /// process automation, and status-based reporting operations.
    /// </summary>
    /// <param name="status">The invoice status to filter by (Draft, Sent, Paid, Overdue, Cancelled)</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with an enumerable collection of status-filtered invoices.</returns>
    Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByStatusAsync(InvoiceStatus status);

    /// <summary>
    /// Retrieves all overdue invoices from the system.
    /// Identifies invoices that are past their due date and not paid,
    /// supporting collections processes and automated reminder systems.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with an enumerable collection of overdue invoices.</returns>
    Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetOverdueInvoicesAsync();

    /// <summary>
    /// Retrieves a specific invoice by its human-readable invoice number.
    /// Provides customer service lookup functionality using the business identifier
    /// instead of the system-generated GUID for user-friendly operations.
    /// </summary>
    /// <param name="invoiceNumber">The invoice number to search for (e.g., "INV1981", "INV2001")</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a RepositoryResult with the invoice if found.</returns>
    Task<RepositoryResult<InvoiceEntity>> GetByInvoiceNumberAsync(string invoiceNumber);
}

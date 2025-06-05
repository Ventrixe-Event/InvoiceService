/**
 * InvoiceService Application Interface - Business Logic Contract
 *
 * Purpose: Defines the contract for invoice management business logic operations
 * Features:
 * - Complete CRUD operations for invoice items
 * - Event and user-specific invoice retrieval
 * - Status-based filtering and overdue invoice tracking
 * - Invoice number-based lookup functionality
 * - Asynchronous operations for optimal performance
 *
 * Architecture: Part of the Application layer in clean architecture
 * - Abstracts business logic from presentation layer
 * - Enables dependency inversion and testability
 * - Supports comprehensive invoice management operations
 * - Facilitates service layer implementation
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using Application.Models;

namespace Application.Interfaces;

/// <summary>
/// Service interface for invoice management business logic operations.
/// Provides a contract for all invoice-related business operations including
/// creation, retrieval, updating, and deletion. Supports various filtering
/// and lookup mechanisms for comprehensive invoice management.
/// </summary>
public interface IInvoiceService
{
    /// <summary>
    /// Retrieves all invoices from the system.
    /// Returns a comprehensive list of invoice items converted to DTOs
    /// for presentation layer consumption.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of invoice DTOs.</returns>
    Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();

    /// <summary>
    /// Retrieves a specific invoice by its unique identifier.
    /// Converts the entity to a DTO for presentation layer consumption
    /// while preserving all relevant invoice information.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the invoice DTO if found, or null if not found.</returns>
    Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all invoices associated with a specific event.
    /// Filters invoices by event ID and returns DTOs suitable for
    /// event-specific financial analysis and reporting.
    /// </summary>
    /// <param name="eventId">The unique identifier of the event to retrieve invoices for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of event-specific invoice DTOs.</returns>
    Task<IEnumerable<InvoiceDto>> GetInvoicesByEventIdAsync(Guid eventId);

    /// <summary>
    /// Retrieves all invoices associated with a specific user.
    /// Filters invoices by user ID and returns DTOs suitable for
    /// user-specific billing history and account management.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve invoices for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of user-specific invoice DTOs.</returns>
    Task<IEnumerable<InvoiceDto>> GetInvoicesByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves all invoices with a specific status.
    /// Filters invoices by status (e.g., "Pending", "Paid", "Overdue") for
    /// status-based reporting and workflow management.
    /// </summary>
    /// <param name="status">The status string to filter invoices by (case-insensitive)</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of status-filtered invoice DTOs.</returns>
    Task<IEnumerable<InvoiceDto>> GetInvoicesByStatusAsync(string status);

    /// <summary>
    /// Retrieves all overdue invoices from the system.
    /// Returns invoices that are past their due date and not marked as paid,
    /// useful for collections and follow-up processes.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of overdue invoice DTOs.</returns>
    Task<IEnumerable<InvoiceDto>> GetOverdueInvoicesAsync();

    /// <summary>
    /// Retrieves a specific invoice by its invoice number.
    /// Provides lookup functionality using the human-readable invoice number
    /// instead of the system-generated GUID identifier.
    /// </summary>
    /// <param name="invoiceNumber">The invoice number to search for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the invoice DTO if found, or null if not found.</returns>
    Task<InvoiceDto?> GetInvoiceByNumberAsync(string invoiceNumber);

    /// <summary>
    /// Creates a new invoice in the system.
    /// Processes the invoice creation request, validates business rules,
    /// and persists the invoice entity to the data store.
    /// </summary>
    /// <param name="createInvoiceDto">The invoice creation request containing all necessary invoice data</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created invoice DTO if successful, or null if creation failed.</returns>
    Task<InvoiceDto?> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto);

    /// <summary>
    /// Updates an existing invoice in the system.
    /// Processes the invoice update request, validates business rules,
    /// and persists the changes to the data store.
    /// </summary>
    /// <param name="updateInvoiceDto">The invoice update request containing the modified invoice data</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated invoice DTO if successful, or null if update failed.</returns>
    Task<InvoiceDto?> UpdateInvoiceAsync(UpdateInvoiceDto updateInvoiceDto);

    /// <summary>
    /// Deletes an invoice from the system.
    /// Removes the invoice and handles any business rule validations
    /// or referential integrity requirements.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice to delete</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if deletion was successful, false otherwise.</returns>
    Task<bool> DeleteInvoiceAsync(Guid id);
}

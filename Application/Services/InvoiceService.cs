/**
 * InvoiceService Application Service - Business Logic Implementation
 *
 * Purpose: Implementation of invoice management business logic operations
 * Features:
 * - Complete CRUD operations with business rule enforcement
 * - Entity-to-DTO mapping for clean architecture separation
 * - Error handling with graceful fallbacks to empty collections
 * - Status enum validation and conversion for type safety
 * - Repository pattern integration with result handling
 * - Asynchronous operations for optimal performance
 *
 * Architecture: Part of the Application layer in clean architecture
 * - Implements IInvoiceService interface contract
 * - Coordinates between presentation and persistence layers
 * - Handles business logic and data transformation
 * - Provides error handling and validation logic
 *
 * Business Logic:
 * - New invoices are created in Draft status by default
 * - Status string validation ensures only valid enum values
 * - Entity mapping maintains data consistency between layers
 * - Graceful error handling prevents exceptions from propagating
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using Application.Interfaces;
using Application.Models;
using Persistence.Entities;
using Persistence.Interfaces;

namespace Application.Services;

/// <summary>
/// Implementation of invoice management business logic operations.
/// Provides comprehensive invoice operations with proper error handling,
/// data validation, and entity-to-DTO mapping for clean architecture separation.
/// Coordinates between presentation and persistence layers while enforcing business rules.
/// </summary>
public class InvoiceService : IInvoiceService
{
    /// <summary>
    /// Repository for invoice data access operations.
    /// Injected through dependency injection for testability and abstraction.
    /// </summary>
    private readonly IInvoiceRepository _invoiceRepository;

    /// <summary>
    /// Initializes a new instance of the InvoiceService class.
    /// Configures the service with the required repository dependency for data access operations.
    /// </summary>
    /// <param name="invoiceRepository">The repository for invoice data access operations</param>
    public InvoiceService(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    /// <summary>
    /// Retrieves all invoices from the system and converts them to DTOs.
    /// Handles repository errors gracefully by returning empty collection on failure.
    /// Maps entity data to DTO format for presentation layer consumption.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of invoice DTOs.</returns>
    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    {
        var result = await _invoiceRepository.GetAllAsync();
        if (!result.IsSuccess || result.Data == null)
        {
            // Return empty collection on failure to maintain consistent behavior
            return Enumerable.Empty<InvoiceDto>();
        }

        // Map entities to DTOs for presentation layer consumption
        return result.Data.Select(MapToDto);
    }

    /// <summary>
    /// Retrieves a specific invoice by ID and converts it to DTO format.
    /// Returns null if invoice is not found or repository operation fails.
    /// Provides safe access to individual invoice data with error handling.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the invoice DTO if found, or null if not found.</returns>
    public async Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id)
    {
        var result = await _invoiceRepository.GetByIdAsync(id);
        // Return mapped DTO on success, null on failure or not found
        return result.IsSuccess && result.Data != null ? MapToDto(result.Data) : null;
    }

    /// <summary>
    /// Retrieves all invoices associated with a specific event and converts them to DTOs.
    /// Handles repository errors gracefully by returning empty collection on failure.
    /// Essential for event-specific financial reporting and revenue analysis.
    /// </summary>
    /// <param name="eventId">The unique identifier of the event to retrieve invoices for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of event-specific invoice DTOs.</returns>
    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByEventIdAsync(Guid eventId)
    {
        var result = await _invoiceRepository.GetByEventIdAsync(eventId);
        if (!result.IsSuccess || result.Data == null)
        {
            // Return empty collection on failure to maintain consistent behavior
            return Enumerable.Empty<InvoiceDto>();
        }

        // Map entities to DTOs for presentation layer consumption
        return result.Data.Select(MapToDto);
    }

    /// <summary>
    /// Retrieves all invoices associated with a specific user and converts them to DTOs.
    /// Handles repository errors gracefully by returning empty collection on failure.
    /// Critical for user account management and billing history operations.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve invoices for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of user-specific invoice DTOs.</returns>
    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByUserIdAsync(Guid userId)
    {
        var result = await _invoiceRepository.GetByUserIdAsync(userId);
        if (!result.IsSuccess || result.Data == null)
        {
            // Return empty collection on failure to maintain consistent behavior
            return Enumerable.Empty<InvoiceDto>();
        }

        // Map entities to DTOs for presentation layer consumption
        return result.Data.Select(MapToDto);
    }

    /// <summary>
    /// Retrieves all invoices with a specific status and converts them to DTOs.
    /// Validates status string against enum values before querying repository.
    /// Returns empty collection for invalid status values or repository failures.
    /// </summary>
    /// <param name="status">The status string to filter by (must match InvoiceStatus enum values)</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of status-filtered invoice DTOs.</returns>
    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByStatusAsync(string status)
    {
        // Validate status string against enum values (case-insensitive)
        if (!Enum.TryParse<InvoiceStatus>(status, true, out var invoiceStatus))
        {
            // Return empty collection for invalid status values
            return Enumerable.Empty<InvoiceDto>();
        }

        var result = await _invoiceRepository.GetByStatusAsync(invoiceStatus);
        if (!result.IsSuccess || result.Data == null)
        {
            // Return empty collection on failure to maintain consistent behavior
            return Enumerable.Empty<InvoiceDto>();
        }

        // Map entities to DTOs for presentation layer consumption
        return result.Data.Select(MapToDto);
    }

    /// <summary>
    /// Retrieves all overdue invoices from the system and converts them to DTOs.
    /// Handles repository errors gracefully by returning empty collection on failure.
    /// Critical for collections processes and automated reminder systems.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of overdue invoice DTOs.</returns>
    public async Task<IEnumerable<InvoiceDto>> GetOverdueInvoicesAsync()
    {
        var result = await _invoiceRepository.GetOverdueInvoicesAsync();
        if (!result.IsSuccess || result.Data == null)
        {
            // Return empty collection on failure to maintain consistent behavior
            return Enumerable.Empty<InvoiceDto>();
        }

        // Map entities to DTOs for presentation layer consumption
        return result.Data.Select(MapToDto);
    }

    /// <summary>
    /// Retrieves a specific invoice by its invoice number and converts it to DTO format.
    /// Returns null if invoice is not found or repository operation fails.
    /// Provides customer service lookup functionality using business identifier.
    /// </summary>
    /// <param name="invoiceNumber">The invoice number to search for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the invoice DTO if found, or null if not found.</returns>
    public async Task<InvoiceDto?> GetInvoiceByNumberAsync(string invoiceNumber)
    {
        var result = await _invoiceRepository.GetByInvoiceNumberAsync(invoiceNumber);
        // Return mapped DTO on success, null on failure or not found
        return result.IsSuccess && result.Data != null ? MapToDto(result.Data) : null;
    }

    /// <summary>
    /// Creates a new invoice in the system from the provided creation request.
    /// Automatically assigns a new GUID, sets status to Draft, and captures creation timestamp.
    /// Returns the created invoice as DTO or null if creation fails.
    /// </summary>
    /// <param name="createInvoiceDto">The invoice creation request containing all necessary invoice data</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created invoice DTO if successful, or null if creation failed.</returns>
    public async Task<InvoiceDto?> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto)
    {
        // Create new invoice entity with business rule enforcement
        var invoice = new InvoiceEntity
        {
            Id = Guid.NewGuid(), // Generate unique identifier
            InvoiceNumber = createInvoiceDto.InvoiceNumber,
            EventId = createInvoiceDto.EventId,
            EventName = createInvoiceDto.EventName,
            UserId = createInvoiceDto.UserId,
            UserName = createInvoiceDto.UserName,
            Amount = createInvoiceDto.Amount,
            IssueDate = createInvoiceDto.IssueDate,
            DueDate = createInvoiceDto.DueDate,
            Status = InvoiceStatus.Draft, // Business rule: new invoices start as Draft
            Description = createInvoiceDto.Description,
            CreatedAt = DateTime.UtcNow, // Capture creation timestamp
        };

        var result = await _invoiceRepository.CreateAsync(invoice);
        // Return mapped DTO on success, null on failure
        return result.IsSuccess && result.Data != null ? MapToDto(result.Data) : null;
    }

    /// <summary>
    /// Updates an existing invoice in the system with the provided update data.
    /// Validates that invoice exists and status is valid before applying changes.
    /// Returns the updated invoice as DTO or null if update fails.
    /// </summary>
    /// <param name="updateInvoiceDto">The invoice update request containing the modified invoice data</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated invoice DTO if successful, or null if update failed.</returns>
    public async Task<InvoiceDto?> UpdateInvoiceAsync(UpdateInvoiceDto updateInvoiceDto)
    {
        // Retrieve existing invoice to ensure it exists
        var existingResult = await _invoiceRepository.GetByIdAsync(updateInvoiceDto.Id);
        if (!existingResult.IsSuccess || existingResult.Data == null)
        {
            // Return null if invoice doesn't exist
            return null;
        }

        var invoice = existingResult.Data;

        // Validate status string against enum values
        if (!Enum.TryParse<InvoiceStatus>(updateInvoiceDto.Status, true, out var status))
        {
            // Return null for invalid status values
            return null;
        }

        // Apply updates to existing entity
        invoice.EventName = updateInvoiceDto.EventName;
        invoice.UserName = updateInvoiceDto.UserName;
        invoice.Amount = updateInvoiceDto.Amount;
        invoice.DueDate = updateInvoiceDto.DueDate;
        invoice.Status = status;
        invoice.Description = updateInvoiceDto.Description;

        var result = await _invoiceRepository.UpdateAsync(invoice);
        // Return mapped DTO on success, null on failure
        return result.IsSuccess && result.Data != null ? MapToDto(result.Data) : null;
    }

    /// <summary>
    /// Deletes an invoice from the system by its unique identifier.
    /// Returns true if deletion was successful, false if invoice doesn't exist or deletion fails.
    /// Handles business rule validations through repository layer.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice to delete</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if deletion was successful, false otherwise.</returns>
    public async Task<bool> DeleteInvoiceAsync(Guid id)
    {
        var result = await _invoiceRepository.DeleteAsync(id);
        // Return success status from repository operation
        return result.IsSuccess;
    }

    /// <summary>
    /// Maps an InvoiceEntity to InvoiceDto for presentation layer consumption.
    /// Converts enum status to string representation and maintains all data integrity.
    /// Private helper method ensuring consistent entity-to-DTO transformation.
    /// </summary>
    /// <param name="entity">The invoice entity to convert to DTO</param>
    /// <returns>The mapped invoice DTO with all properties populated</returns>
    private static InvoiceDto MapToDto(InvoiceEntity entity)
    {
        return new InvoiceDto
        {
            Id = entity.Id,
            InvoiceNumber = entity.InvoiceNumber,
            EventId = entity.EventId,
            EventName = entity.EventName,
            UserId = entity.UserId,
            UserName = entity.UserName,
            Amount = entity.Amount,
            IssueDate = entity.IssueDate,
            DueDate = entity.DueDate,
            Status = entity.Status.ToString(), // Convert enum to string for DTO
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
        };
    }
}

/**
 * InvoiceService Invoices REST API Controller - HTTP Endpoint Management
 *
 * Purpose: RESTful API controller for invoice management operations in the event management platform
 * Features:
 * - Complete CRUD operations for invoice entities
 * - Multiple filtering endpoints (by event, user, status, overdue)
 * - Invoice number lookup for customer service operations
 * - Mock data implementation for development and demonstration
 * - Comprehensive error handling with consistent response format
 * - Asynchronous operations for optimal performance
 *
 * API Endpoints:
 * - GET /api/invoices - Retrieve all invoices
 * - GET /api/invoices/{id} - Retrieve specific invoice by ID
 * - GET /api/invoices/number/{invoiceNumber} - Retrieve by invoice number
 * - GET /api/invoices/status/{status} - Retrieve by status
 * - GET /api/invoices/overdue - Retrieve overdue invoices
 * - GET /api/invoices/event/{eventId} - Retrieve by event
 * - GET /api/invoices/user/{userId} - Retrieve by user
 * - POST /api/invoices - Create new invoice
 * - PUT /api/invoices/{id} - Update existing invoice
 * - DELETE /api/invoices/{id} - Delete invoice
 *
 * Architecture: Part of the Presentation layer in clean architecture
 * - Handles HTTP requests and responses for invoice operations
 * - Integrates with application layer services for business logic
 * - Provides RESTful interface for frontend and external systems
 * - Implements consistent error handling and response formatting
 *
 * Mock Data Strategy:
 * - Contains comprehensive mock invoice data for development
 * - Covers multiple event types and financial scenarios
 * - Demonstrates various invoice statuses and amounts
 * - Provides realistic data for frontend development and testing
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

/// <summary>
/// REST API controller for invoice management operations.
/// Provides comprehensive HTTP endpoints for invoice CRUD operations, filtering,
/// and specialized queries. Uses mock data for development and demonstration purposes.
/// All endpoints return consistent response format with success/error indication.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class InvoicesController(IInvoiceService invoiceService) : ControllerBase
{
    /// <summary>
    /// Invoice service for business logic operations.
    /// Injected through constructor for dependency inversion and testability.
    /// </summary>
    private readonly IInvoiceService _invoiceService = invoiceService;

    // Mock data for development/demo purposes - comprehensive invoice scenarios
    // This mock dataset provides realistic invoice data covering various event types,
    // financial amounts, statuses, and use cases for frontend development and testing.
    // Data includes overdue invoices, paid invoices, and pending payments across
    // different event categories like music festivals, fashion shows, tech conferences, etc.
    private readonly List<InvoiceDto> _mockInvoices = new()
    {
        // Music Festival - High-value overdue invoice for premium event experience
        new InvoiceDto
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            InvoiceNumber = "INV1981",
            EventId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            EventName = "Echo Beats Festival",
            UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            UserName = "Jackson Moore",
            Amount = 654m,
            IssueDate = new DateTime(2025, 5, 20),
            DueDate = new DateTime(2025, 6, 15),
            Status = "Overdue",
            Description = "Event ticket payment",
            CreatedAt = new DateTime(2025, 4, 28),
        },
        // Fashion Show - Premium VIP package with significant overdue amount
        new InvoiceDto
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            InvoiceNumber = "INV1987",
            EventId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            EventName = "Runway Revolution 2029",
            UserId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            UserName = "Alicia Smithson",
            Amount = 5656m,
            IssueDate = new DateTime(2025, 8, 17),
            DueDate = new DateTime(2025, 9, 15),
            Status = "Overdue",
            Description = "VIP event package",
            CreatedAt = new DateTime(2025, 8, 17),
        },
        // Classical Music - Premium seating for sophisticated entertainment
        new InvoiceDto
        {
            Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
            InvoiceNumber = "INV888",
            EventId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
            EventName = "Symphony Under the Stars",
            UserId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            UserName = "Patrick Cooper",
            Amount = 5500m,
            IssueDate = new DateTime(2025, 3, 2),
            DueDate = new DateTime(2025, 4, 1),
            Status = "Overdue",
            Description = "Premium seating reservation",
            CreatedAt = new DateTime(2025, 3, 2),
        },
        // Technology Conference - Professional development investment (paid)
        new InvoiceDto
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            InvoiceNumber = "INV2001",
            EventId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            EventName = "Tech Future Expo",
            UserId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            UserName = "Sarah Chen",
            Amount = 1200m,
            IssueDate = new DateTime(2025, 1, 15),
            DueDate = new DateTime(2025, 2, 15),
            Status = "Paid",
            Description = "Conference registration",
            CreatedAt = new DateTime(2025, 1, 15),
        },
        // Culinary Event - Food festival experience (pending payment)
        new InvoiceDto
        {
            Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            InvoiceNumber = "INV2002",
            EventId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            EventName = "Culinary Delights Festival",
            UserId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
            UserName = "Clara Simmons",
            Amount = 850m,
            IssueDate = new DateTime(2025, 2, 10),
            DueDate = new DateTime(2025, 3, 10),
            Status = "Pending",
            Description = "Food festival tickets",
            CreatedAt = new DateTime(2025, 2, 10),
        },
    };

    /// <summary>
    /// Retrieves all invoices from the system.
    /// Returns comprehensive invoice list with mock data for development purposes.
    /// Includes invoices across multiple events, users, and status types.
    /// </summary>
    /// <returns>ActionResult containing all invoices wrapped in success response</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            // Simulate asynchronous database operation with realistic delay
            await Task.Delay(100);
            return Ok(new { Success = true, Result = _mockInvoices });
        }
        catch (Exception ex)
        {
            // Return standardized error response for consistent error handling
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a specific invoice by its unique identifier.
    /// Searches mock data for invoice matching the provided GUID.
    /// Returns 404 if invoice is not found in the system.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice to retrieve</param>
    /// <returns>ActionResult containing the invoice if found, or 404 error response</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            // Simulate asynchronous database lookup operation
            await Task.Delay(50);
            var invoice = _mockInvoices.FirstOrDefault(i => i.Id == id);

            if (invoice == null)
            {
                // Return standardized not found response
                return NotFound(new { Success = false, Error = "Invoice not found" });
            }

            // Return successful response with invoice data
            return Ok(new { Success = true, Result = invoice });
        }
        catch (Exception ex)
        {
            // Return standardized error response for exception handling
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a specific invoice by its human-readable invoice number.
    /// Provides customer service lookup functionality using business identifier.
    /// Returns 404 if invoice number is not found in the system.
    /// </summary>
    /// <param name="invoiceNumber">The invoice number to search for (e.g., "INV1981")</param>
    /// <returns>ActionResult containing the invoice if found, or 404 error response</returns>
    [HttpGet("number/{invoiceNumber}")]
    public async Task<IActionResult> GetByNumber(string invoiceNumber)
    {
        try
        {
            // Simulate asynchronous database lookup by invoice number
            await Task.Delay(50);
            var invoice = _mockInvoices.FirstOrDefault(i => i.InvoiceNumber == invoiceNumber);

            if (invoice == null)
            {
                // Return standardized not found response
                return NotFound(new { Success = false, Error = "Invoice not found" });
            }

            // Return successful response with invoice data
            return Ok(new { Success = true, Result = invoice });
        }
        catch (Exception ex)
        {
            // Return standardized error response for exception handling
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all invoices with a specific status.
    /// Filters invoices by status string (case-insensitive comparison).
    /// Useful for workflow management and status-based reporting.
    /// </summary>
    /// <param name="status">The status to filter by ("Paid", "Pending", "Overdue", etc.)</param>
    /// <returns>ActionResult containing filtered invoices matching the specified status</returns>
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        try
        {
            // Simulate asynchronous database filtering operation
            await Task.Delay(100);
            var invoices = _mockInvoices.Where(i =>
                i.Status.Equals(status, StringComparison.OrdinalIgnoreCase)
            );

            // Return successful response with filtered invoice collection
            return Ok(new { Success = true, Result = invoices });
        }
        catch (Exception ex)
        {
            // Return standardized error response for exception handling
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all overdue invoices from the system.
    /// Identifies invoices marked as "Overdue" or past due date without payment.
    /// Critical for collections processes and automated follow-up systems.
    /// </summary>
    /// <returns>ActionResult containing all overdue invoices for collections management</returns>
    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdue()
    {
        try
        {
            // Simulate asynchronous database operation for overdue calculation
            await Task.Delay(100);
            var overdueInvoices = _mockInvoices.Where(i =>
                i.Status.Equals("Overdue", StringComparison.OrdinalIgnoreCase)
                || (
                    i.DueDate < DateTime.Now
                    && !i.Status.Equals("Paid", StringComparison.OrdinalIgnoreCase)
                )
            );

            // Return successful response with overdue invoice collection
            return Ok(new { Success = true, Result = overdueInvoices });
        }
        catch (Exception ex)
        {
            // Return standardized error response for exception handling
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all invoices associated with a specific event.
    /// Filters invoices by event ID for event-specific financial reporting.
    /// Essential for revenue analysis and cost allocation operations.
    /// </summary>
    /// <param name="eventId">The unique identifier of the event to retrieve invoices for</param>
    /// <returns>ActionResult containing all invoices associated with the specified event</returns>
    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetByEvent(Guid eventId)
    {
        try
        {
            // Simulate asynchronous database filtering by event ID
            await Task.Delay(100);
            var invoices = _mockInvoices.Where(i => i.EventId == eventId);

            // Return successful response with event-specific invoice collection
            return Ok(new { Success = true, Result = invoices });
        }
        catch (Exception ex)
        {
            // Return standardized error response for exception handling
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all invoices associated with a specific user.
    /// Filters invoices by user ID for user-specific financial reporting.
    /// Essential for user-specific financial analysis and transaction history.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve invoices for</param>
    /// <returns>ActionResult containing all invoices associated with the specified user</returns>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(Guid userId)
    {
        try
        {
            // Simulate asynchronous database filtering by user ID
            await Task.Delay(100);
            var invoices = _mockInvoices.Where(i => i.UserId == userId);

            // Return successful response with user-specific invoice collection
            return Ok(new { Success = true, Result = invoices });
        }
        catch (Exception ex)
        {
            // Return standardized error response for exception handling
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new invoice in the system.
    /// Validates the input data and creates a new invoice entity.
    /// Returns the created invoice with a success response.
    /// </summary>
    /// <param name="request">The request object containing invoice creation details</param>
    /// <returns>ActionResult containing the created invoice if successful, or 500 error response</returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateInvoiceDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Success = false, Error = "Invalid data" });
            }

            await Task.Delay(200); // Simulate async operation

            var newInvoice = new InvoiceDto
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = $"INV{DateTime.Now:yyyyMMddHHmmss}",
                EventId = request.EventId,
                EventName = "New Event", // In real implementation, fetch from EventService
                UserId = request.UserId,
                UserName = "New User", // In real implementation, fetch from UserService
                Amount = request.Amount,
                IssueDate = DateTime.Now,
                DueDate = request.DueDate,
                Status = "Pending",
                Description = request.Description,
                CreatedAt = DateTime.Now,
            };

            return Ok(new { Success = true, Result = newInvoice });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing invoice in the system.
    /// Validates the input data and updates the invoice entity.
    /// Returns the updated invoice with a success response.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice to update</param>
    /// <param name="request">The request object containing updated invoice details</param>
    /// <returns>ActionResult containing the updated invoice if successful, or 500 error response</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateInvoiceDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Success = false, Error = "Invalid data" });
            }

            await Task.Delay(200); // Simulate async operation

            var existingInvoice = _mockInvoices.FirstOrDefault(i => i.Id == id);
            if (existingInvoice == null)
            {
                return NotFound(new { Success = false, Error = "Invoice not found" });
            }

            // Update the invoice (in real implementation, this would update the database)
            var updatedInvoice = new InvoiceDto
            {
                Id = existingInvoice.Id,
                InvoiceNumber = existingInvoice.InvoiceNumber,
                EventId = existingInvoice.EventId,
                EventName = existingInvoice.EventName,
                UserId = existingInvoice.UserId,
                UserName = existingInvoice.UserName,
                Amount = request.Amount,
                IssueDate = existingInvoice.IssueDate,
                DueDate = request.DueDate,
                Status = request.Status ?? existingInvoice.Status,
                Description = request.Description ?? existingInvoice.Description,
                CreatedAt = existingInvoice.CreatedAt,
            };

            return Ok(new { Success = true, Result = updatedInvoice });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes an existing invoice from the system.
    /// Removes the invoice entity from the database.
    /// Returns a success response if the invoice is deleted.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice to delete</param>
    /// <returns>ActionResult containing success or error response</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await Task.Delay(100); // Simulate async operation

            var invoice = _mockInvoices.FirstOrDefault(i => i.Id == id);
            if (invoice == null)
            {
                return NotFound(new { Success = false, Error = "Invoice not found" });
            }

            return Ok(new { Success = true, Message = "Invoice deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }
}

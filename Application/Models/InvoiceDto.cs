/**
 * InvoiceService Invoice DTO - Data Transfer Object
 *
 * Purpose: Data Transfer Object for invoice information exchange between layers
 * Features:
 * - Comprehensive invoice representation for API responses
 * - Event and user association information
 * - Financial data with amount and date tracking
 * - Status management for invoice workflow
 * - Timestamp tracking for audit purposes
 *
 * Architecture: Part of the Application layer in clean architecture
 * - Encapsulates invoice data for transfer between layers
 * - Provides clean API contract for frontend consumption
 * - Abstracts internal entity structure from external interfaces
 * - Supports financial management and billing operations
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

namespace Application.Models;

/// <summary>
/// Data Transfer Object representing invoice information for API responses.
/// Provides a comprehensive view of invoice data optimized for frontend consumption
/// including financial details, status tracking, and associated entity information.
/// Used for displaying invoice lists, details, and financial reporting.
/// </summary>
public class InvoiceDto
{
    /// <summary>
    /// Gets or sets the unique identifier for this invoice.
    /// Generated automatically when invoice is created in the system.
    /// Used for internal referencing and API operations.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the human-readable invoice number.
    /// Unique identifier used for customer communication and reference.
    /// Typically follows a specific format (e.g., "INV1981", "INV2001").
    /// </summary>
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the event this invoice relates to.
    /// Links the invoice to specific events for financial tracking and reporting.
    /// Used for event-specific revenue analysis and cost allocation.
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Gets or sets the human-readable name of the event this invoice relates to.
    /// Denormalized for efficiency to avoid additional lookups during display.
    /// Provides context when viewing invoices without additional event queries.
    /// </summary>
    public string EventName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the user who owns this invoice.
    /// Links the invoice to specific users for account management and billing.
    /// Used for user-specific financial tracking and payment processing.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the user who owns this invoice.
    /// Denormalized for efficiency to avoid user service lookups during display.
    /// Provides human-readable user identification for invoice attribution.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the monetary amount of this invoice.
    /// Represents the total amount due in the system's base currency.
    /// Used for financial calculations, reporting, and payment processing.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the date when this invoice was issued.
    /// Marks the official start of the payment period and billing cycle.
    /// Used for calculating payment terms and aging reports.
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the date when payment for this invoice is due.
    /// Defines the deadline for payment and determines overdue status.
    /// Used for payment reminders, overdue calculations, and collections.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the current status of this invoice.
    /// Common values: "Pending", "Paid", "Overdue", "Cancelled".
    /// Used for workflow management, filtering, and business process automation.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional description of this invoice.
    /// Provides additional context about the invoice purpose or contents.
    /// Examples: "Event ticket payment", "VIP event package", "Conference registration".
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this invoice was created in the system.
    /// Automatically set when invoice is generated for audit and tracking purposes.
    /// Used for chronological ordering and creation time analysis.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

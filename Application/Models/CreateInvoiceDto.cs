/**
 * InvoiceService Create Invoice Request - Input Validation Model
 *
 * Purpose: Request model for creating new invoices with comprehensive validation
 * Features:
 * - Required field validation for essential invoice data
 * - Amount validation with positive value constraints
 * - String length validation for data integrity
 * - Date validation for proper billing cycles
 * - Business rule enforcement through validation attributes
 *
 * Architecture: Part of the Application layer in clean architecture
 * - Encapsulates input validation and business rules
 * - Provides data transfer object for invoice creation
 * - Supports financial workflow management
 * - Includes comprehensive validation for data integrity
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using System.ComponentModel.DataAnnotations;

namespace Application.Models;

/// <summary>
/// Request model for creating new invoices in the system.
/// Includes comprehensive validation rules for financial data integrity and business rule enforcement.
/// All required fields must be provided with valid data to ensure proper invoice generation.
/// Validation attributes ensure data consistency and prevent invalid invoice creation.
/// </summary>
public class CreateInvoiceDto
{
    /// <summary>
    /// Gets or sets the human-readable invoice number for this invoice.
    /// Required field that serves as the primary customer reference identifier.
    /// Must be unique within the system and follow organizational numbering conventions.
    /// Maximum length of 50 characters to ensure database compatibility.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the event this invoice relates to.
    /// Required field that links the invoice to a specific event for financial tracking.
    /// Must reference a valid event that exists in the system.
    /// </summary>
    [Required]
    public Guid EventId { get; set; }

    /// <summary>
    /// Gets or sets the human-readable name of the event this invoice relates to.
    /// Required field that provides context and improves user experience.
    /// Denormalized to avoid additional lookups during invoice display.
    /// Maximum length of 255 characters for adequate event name storage.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string EventName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the user who will receive this invoice.
    /// Required field that establishes billing responsibility and account association.
    /// Must reference a valid user that exists in the system.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the user who will receive this invoice.
    /// Required field that provides human-readable user identification.
    /// Denormalized to avoid user service lookups during invoice processing.
    /// Maximum length of 255 characters for adequate user name storage.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the monetary amount for this invoice.
    /// Required field that must be greater than 0 to ensure valid financial transactions.
    /// Represents the total amount due in the system's base currency.
    /// Range validation prevents negative amounts and zero-value invoices.
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the date when this invoice is issued.
    /// Required field that marks the official start of the billing cycle.
    /// Used for calculating payment terms and establishing the invoice timeline.
    /// Should typically be set to the current date or a future effective date.
    /// </summary>
    [Required]
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the date when payment for this invoice is due.
    /// Required field that establishes the payment deadline for this invoice.
    /// Must be after the issue date to ensure proper payment terms.
    /// Used for overdue calculations and payment reminder scheduling.
    /// </summary>
    [Required]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the optional description for this invoice.
    /// Provides additional context about the invoice purpose, contents, or special instructions.
    /// Maximum length of 1000 characters allows for detailed explanations.
    /// Examples: "Event ticket payment for premium seating", "VIP package with backstage access".
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
}

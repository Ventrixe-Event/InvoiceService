/**
 * InvoiceService Update Invoice Request - Input Validation Model
 *
 * Purpose: Request model for updating existing invoices with comprehensive validation
 * Features:
 * - Required field validation for essential updatable invoice data
 * - Amount validation with positive value constraints
 * - String length validation for data integrity
 * - Status management for invoice workflow updates
 * - Business rule enforcement through validation attributes
 *
 * Architecture: Part of the Application layer in clean architecture
 * - Encapsulates input validation and business rules for updates
 * - Provides data transfer object for invoice modification
 * - Supports financial workflow and status management
 * - Excludes non-updatable fields like creation timestamps
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using System.ComponentModel.DataAnnotations;

namespace Application.Models;

/// <summary>
/// Request model for updating existing invoices in the system.
/// Includes validation rules for updatable fields while maintaining data integrity.
/// Contains only fields that are allowed to be modified after invoice creation.
/// Validation attributes ensure data consistency and prevent invalid updates.
/// </summary>
public class UpdateInvoiceDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the invoice to update.
    /// Required field that specifies which invoice should be modified.
    /// Must reference an existing invoice in the system.
    /// Used for identifying the target invoice during update operations.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the updated human-readable name of the event this invoice relates to.
    /// Required field that allows correction of event information without changing the event ID.
    /// Useful for updating event details that may have changed after invoice creation.
    /// Maximum length of 255 characters for adequate event name storage.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string EventName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated display name of the user who owns this invoice.
    /// Required field that allows correction of user information without changing the user ID.
    /// Useful for updating user details that may have changed after invoice creation.
    /// Maximum length of 255 characters for adequate user name storage.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated monetary amount for this invoice.
    /// Required field that must be greater than 0 to ensure valid financial transactions.
    /// Allows modification of invoice amount for corrections or adjustments.
    /// Range validation prevents negative amounts and zero-value invoices.
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the updated due date for this invoice.
    /// Required field that allows modification of payment deadlines.
    /// Useful for extending payment terms or correcting due date errors.
    /// Should maintain reasonable business logic relative to issue date.
    /// </summary>
    [Required]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the updated status of this invoice.
    /// Required field that enables invoice workflow management and status tracking.
    /// Common values: "Pending", "Paid", "Overdue", "Cancelled".
    /// Critical for payment processing workflows and business process automation.
    /// </summary>
    [Required]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated optional description for this invoice.
    /// Allows modification of invoice description for clarification or additional details.
    /// Maximum length of 1000 characters allows for detailed explanations.
    /// Can be used to add payment instructions, special terms, or updated information.
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
}

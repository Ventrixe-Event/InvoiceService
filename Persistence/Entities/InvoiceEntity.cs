/**
 * InvoiceService Invoice Entity - Database Entity Model
 *
 * Purpose: Entity Framework Core model representing invoice data in the database
 * Features:
 * - Complete invoice data representation with EF Core annotations
 * - Financial data with precise decimal storage (18,2)
 * - Status management using enum with database conversion
 * - Unique constraint on invoice number for business rule enforcement
 * - Comprehensive validation attributes for data integrity
 *
 * Architecture: Part of the Persistence layer in clean architecture
 * - Maps directly to database table structure
 * - Encapsulates invoice data persistence logic
 * - Supports Entity Framework Core operations and migrations
 * - Provides foundation for repository pattern implementation
 *
 * Database Schema:
 * - Table: Invoices
 * - Primary Key: Id (GUID, auto-generated)
 * - Unique Index: InvoiceNumber
 * - Foreign Keys: EventId, UserId (logical references)
 * - Precision: Amount stored as decimal(18,2)
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities;

/// <summary>
/// Entity Framework Core entity representing invoice data in the database.
/// Maps to the Invoices table and includes all financial and tracking information
/// required for comprehensive invoice management and billing operations.
/// Includes validation attributes and database-specific configurations.
/// </summary>
public class InvoiceEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for this invoice entity.
    /// Serves as the primary key in the database with auto-generation.
    /// Used for internal referencing and foreign key relationships.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the human-readable invoice number.
    /// Required field with maximum length of 50 characters.
    /// Has unique constraint enforced at database level to prevent duplicates.
    /// Used for customer communication and business reference.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the event this invoice relates to.
    /// Required field that establishes logical foreign key relationship to events.
    /// Used for event-specific financial reporting and revenue tracking.
    /// </summary>
    [Required]
    public Guid EventId { get; set; }

    /// <summary>
    /// Gets or sets the human-readable name of the event this invoice relates to.
    /// Required field with maximum length of 255 characters.
    /// Denormalized for query efficiency and reduced join operations.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string EventName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the user who owns this invoice.
    /// Required field that establishes logical foreign key relationship to users.
    /// Used for user-specific billing and account management operations.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the user who owns this invoice.
    /// Required field with maximum length of 255 characters.
    /// Denormalized for query efficiency and reduced external service calls.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the monetary amount of this invoice.
    /// Required field stored as decimal(18,2) for precise financial calculations.
    /// Supports amounts up to 999,999,999,999,999,999.99 with two decimal places.
    /// Column type annotation ensures proper SQL Server decimal precision.
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the date when this invoice was issued.
    /// Required field that marks the official start of the billing cycle.
    /// Stored as datetime2 in SQL Server for high precision and range.
    /// </summary>
    [Required]
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the date when payment for this invoice is due.
    /// Required field that establishes the payment deadline.
    /// Used for overdue calculations and automated reminder systems.
    /// </summary>
    [Required]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the current status of this invoice using enum values.
    /// Required field with automatic conversion to integer for database storage.
    /// Supports workflow states: Draft, Sent, Paid, Overdue, Cancelled.
    /// Enum conversion configured in DataContext for type safety.
    /// </summary>
    [Required]
    public InvoiceStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the optional description of this invoice.
    /// Maximum length of 1000 characters for detailed invoice information.
    /// Can contain payment instructions, special terms, or additional context.
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this invoice was created in the system.
    /// Required field automatically set during entity creation for audit purposes.
    /// Stored as datetime2 in SQL Server for high precision timestamp tracking.
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; }
}

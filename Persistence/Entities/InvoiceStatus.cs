/**
 * InvoiceService Invoice Status Enumeration - Workflow State Management
 *
 * Purpose: Enumeration defining the possible states of an invoice in the billing workflow
 * Features:
 * - Complete invoice lifecycle state representation
 * - Ordered workflow progression from draft to completion
 * - Database-friendly integer mapping for efficient storage
 * - Type-safe status management for business logic
 * - Support for invoice tracking and reporting operations
 *
 * Architecture: Part of the Persistence layer in clean architecture
 * - Provides type safety for invoice status management
 * - Integrates with Entity Framework Core enum conversion
 * - Supports business process automation and workflow rules
 * - Enables status-based filtering and reporting queries
 *
 * Workflow States:
 * - Draft: Initial creation state, not yet sent to customer
 * - Sent: Invoice has been sent to customer, awaiting payment
 * - Paid: Payment received and processed successfully
 * - Overdue: Payment deadline has passed without payment
 * - Cancelled: Invoice has been cancelled and is no longer valid
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

namespace Persistence.Entities;

/// <summary>
/// Enumeration representing the possible states of an invoice in the billing workflow.
/// Provides type-safe status management with integer values for efficient database storage.
/// Each status represents a specific stage in the invoice lifecycle from creation to completion.
/// Used for workflow management, automated processing, and business rule enforcement.
/// </summary>
public enum InvoiceStatus
{
    /// <summary>
    /// Invoice is in draft state - created but not yet sent to the customer.
    /// Initial state when invoice is being prepared or reviewed before sending.
    /// Invoice can be edited freely while in this state.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// Invoice has been sent to the customer and is awaiting payment.
    /// Customer has been notified and payment deadline is active.
    /// Invoice should not be modified while in this state.
    /// </summary>
    Sent = 1,

    /// <summary>
    /// Payment has been received and processed successfully.
    /// Final state for completed transactions with successful payment.
    /// No further action required for this invoice.
    /// </summary>
    Paid = 2,

    /// <summary>
    /// Payment deadline has passed without receiving payment.
    /// Triggers collection processes and overdue notifications.
    /// May require follow-up actions or payment plan arrangements.
    /// </summary>
    Overdue = 3,

    /// <summary>
    /// Invoice has been cancelled and is no longer valid.
    /// Used when invoice needs to be voided due to changes or errors.
    /// No payment is expected and no collection activities should occur.
    /// </summary>
    Cancelled = 4,
}

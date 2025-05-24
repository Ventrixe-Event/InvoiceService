using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class CreateInvoiceDto
{
    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required]
    public Guid EventId { get; set; }

    [Required]
    [MaxLength(255)]
    public string EventName { get; set; } = string.Empty;

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(255)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime IssueDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class UpdateInvoiceDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string EventName { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }
}

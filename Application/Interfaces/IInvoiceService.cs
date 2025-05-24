using Application.Models;

namespace Application.Interfaces;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
    Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id);
    Task<IEnumerable<InvoiceDto>> GetInvoicesByEventIdAsync(Guid eventId);
    Task<IEnumerable<InvoiceDto>> GetInvoicesByUserIdAsync(Guid userId);
    Task<IEnumerable<InvoiceDto>> GetInvoicesByStatusAsync(string status);
    Task<IEnumerable<InvoiceDto>> GetOverdueInvoicesAsync();
    Task<InvoiceDto?> GetInvoiceByNumberAsync(string invoiceNumber);
    Task<InvoiceDto?> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto);
    Task<InvoiceDto?> UpdateInvoiceAsync(UpdateInvoiceDto updateInvoiceDto);
    Task<bool> DeleteInvoiceAsync(Guid id);
}

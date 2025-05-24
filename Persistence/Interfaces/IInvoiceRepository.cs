using Persistence.Entities;
using Persistence.Models;

namespace Persistence.Interfaces;

public interface IInvoiceRepository : IBaseRepository<InvoiceEntity>
{
    Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByEventIdAsync(Guid eventId);
    Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByUserIdAsync(Guid userId);
    Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByStatusAsync(InvoiceStatus status);
    Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetOverdueInvoicesAsync();
    Task<RepositoryResult<InvoiceEntity>> GetByInvoiceNumberAsync(string invoiceNumber);
}

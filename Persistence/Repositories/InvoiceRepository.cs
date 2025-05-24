using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Entities;
using Persistence.Interfaces;
using Persistence.Models;

namespace Persistence.Repositories;

public class InvoiceRepository : BaseRepository<InvoiceEntity>, IInvoiceRepository
{
    public InvoiceRepository(DataContext context)
        : base(context) { }

    public async Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByEventIdAsync(Guid eventId)
    {
        try
        {
            var invoices = await _dbSet
                .Where(i => i.EventId == eventId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return RepositoryResult<IEnumerable<InvoiceEntity>>.Success(invoices);
        }
        catch (Exception ex)
        {
            return RepositoryResult<IEnumerable<InvoiceEntity>>.Failure(
                $"Error retrieving invoices by event ID: {ex.Message}"
            );
        }
    }

    public async Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByUserIdAsync(Guid userId)
    {
        try
        {
            var invoices = await _dbSet
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return RepositoryResult<IEnumerable<InvoiceEntity>>.Success(invoices);
        }
        catch (Exception ex)
        {
            return RepositoryResult<IEnumerable<InvoiceEntity>>.Failure(
                $"Error retrieving invoices by user ID: {ex.Message}"
            );
        }
    }

    public async Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetByStatusAsync(
        InvoiceStatus status
    )
    {
        try
        {
            var invoices = await _dbSet
                .Where(i => i.Status == status)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return RepositoryResult<IEnumerable<InvoiceEntity>>.Success(invoices);
        }
        catch (Exception ex)
        {
            return RepositoryResult<IEnumerable<InvoiceEntity>>.Failure(
                $"Error retrieving invoices by status: {ex.Message}"
            );
        }
    }

    public async Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetOverdueInvoicesAsync()
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var invoices = await _dbSet
                .Where(i =>
                    i.DueDate < today
                    && i.Status != InvoiceStatus.Paid
                    && i.Status != InvoiceStatus.Cancelled
                )
                .OrderBy(i => i.DueDate)
                .ToListAsync();

            return RepositoryResult<IEnumerable<InvoiceEntity>>.Success(invoices);
        }
        catch (Exception ex)
        {
            return RepositoryResult<IEnumerable<InvoiceEntity>>.Failure(
                $"Error retrieving overdue invoices: {ex.Message}"
            );
        }
    }

    public async Task<RepositoryResult<InvoiceEntity>> GetByInvoiceNumberAsync(string invoiceNumber)
    {
        try
        {
            var invoice = await _dbSet.FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);

            if (invoice == null)
            {
                return RepositoryResult<InvoiceEntity>.Failure("Invoice not found");
            }

            return RepositoryResult<InvoiceEntity>.Success(invoice);
        }
        catch (Exception ex)
        {
            return RepositoryResult<InvoiceEntity>.Failure(
                $"Error retrieving invoice by number: {ex.Message}"
            );
        }
    }

    public override async Task<RepositoryResult<IEnumerable<InvoiceEntity>>> GetAllAsync()
    {
        try
        {
            var invoices = await _dbSet.OrderByDescending(i => i.CreatedAt).ToListAsync();

            return RepositoryResult<IEnumerable<InvoiceEntity>>.Success(invoices);
        }
        catch (Exception ex)
        {
            return RepositoryResult<IEnumerable<InvoiceEntity>>.Failure(
                $"Error retrieving all invoices: {ex.Message}"
            );
        }
    }
}

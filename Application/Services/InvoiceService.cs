using Application.Interfaces;
using Application.Models;
using Persistence.Entities;
using Persistence.Interfaces;

namespace Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    {
        var result = await _invoiceRepository.GetAllAsync();
        if (!result.IsSuccess || result.Data == null)
        {
            return Enumerable.Empty<InvoiceDto>();
        }

        return result.Data.Select(MapToDto);
    }

    public async Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id)
    {
        var result = await _invoiceRepository.GetByIdAsync(id);
        return result.IsSuccess && result.Data != null ? MapToDto(result.Data) : null;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByEventIdAsync(Guid eventId)
    {
        var result = await _invoiceRepository.GetByEventIdAsync(eventId);
        if (!result.IsSuccess || result.Data == null)
        {
            return Enumerable.Empty<InvoiceDto>();
        }

        return result.Data.Select(MapToDto);
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByUserIdAsync(Guid userId)
    {
        var result = await _invoiceRepository.GetByUserIdAsync(userId);
        if (!result.IsSuccess || result.Data == null)
        {
            return Enumerable.Empty<InvoiceDto>();
        }

        return result.Data.Select(MapToDto);
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByStatusAsync(string status)
    {
        if (!Enum.TryParse<InvoiceStatus>(status, true, out var invoiceStatus))
        {
            return Enumerable.Empty<InvoiceDto>();
        }

        var result = await _invoiceRepository.GetByStatusAsync(invoiceStatus);
        if (!result.IsSuccess || result.Data == null)
        {
            return Enumerable.Empty<InvoiceDto>();
        }

        return result.Data.Select(MapToDto);
    }

    public async Task<IEnumerable<InvoiceDto>> GetOverdueInvoicesAsync()
    {
        var result = await _invoiceRepository.GetOverdueInvoicesAsync();
        if (!result.IsSuccess || result.Data == null)
        {
            return Enumerable.Empty<InvoiceDto>();
        }

        return result.Data.Select(MapToDto);
    }

    public async Task<InvoiceDto?> GetInvoiceByNumberAsync(string invoiceNumber)
    {
        var result = await _invoiceRepository.GetByInvoiceNumberAsync(invoiceNumber);
        return result.IsSuccess && result.Data != null ? MapToDto(result.Data) : null;
    }

    public async Task<InvoiceDto?> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto)
    {
        var invoice = new InvoiceEntity
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = createInvoiceDto.InvoiceNumber,
            EventId = createInvoiceDto.EventId,
            EventName = createInvoiceDto.EventName,
            UserId = createInvoiceDto.UserId,
            UserName = createInvoiceDto.UserName,
            Amount = createInvoiceDto.Amount,
            IssueDate = createInvoiceDto.IssueDate,
            DueDate = createInvoiceDto.DueDate,
            Status = InvoiceStatus.Draft,
            Description = createInvoiceDto.Description,
            CreatedAt = DateTime.UtcNow,
        };

        var result = await _invoiceRepository.CreateAsync(invoice);
        return result.IsSuccess && result.Data != null ? MapToDto(result.Data) : null;
    }

    public async Task<InvoiceDto?> UpdateInvoiceAsync(UpdateInvoiceDto updateInvoiceDto)
    {
        var existingResult = await _invoiceRepository.GetByIdAsync(updateInvoiceDto.Id);
        if (!existingResult.IsSuccess || existingResult.Data == null)
        {
            return null;
        }

        var invoice = existingResult.Data;
        if (!Enum.TryParse<InvoiceStatus>(updateInvoiceDto.Status, true, out var status))
        {
            return null;
        }

        invoice.EventName = updateInvoiceDto.EventName;
        invoice.UserName = updateInvoiceDto.UserName;
        invoice.Amount = updateInvoiceDto.Amount;
        invoice.DueDate = updateInvoiceDto.DueDate;
        invoice.Status = status;
        invoice.Description = updateInvoiceDto.Description;

        var result = await _invoiceRepository.UpdateAsync(invoice);
        return result.IsSuccess && result.Data != null ? MapToDto(result.Data) : null;
    }

    public async Task<bool> DeleteInvoiceAsync(Guid id)
    {
        var result = await _invoiceRepository.DeleteAsync(id);
        return result.IsSuccess;
    }

    private static InvoiceDto MapToDto(InvoiceEntity entity)
    {
        return new InvoiceDto
        {
            Id = entity.Id,
            InvoiceNumber = entity.InvoiceNumber,
            EventId = entity.EventId,
            EventName = entity.EventName,
            UserId = entity.UserId,
            UserName = entity.UserName,
            Amount = entity.Amount,
            IssueDate = entity.IssueDate,
            DueDate = entity.DueDate,
            Status = entity.Status.ToString(),
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
        };
    }
}

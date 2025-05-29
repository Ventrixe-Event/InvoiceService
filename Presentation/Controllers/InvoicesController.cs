using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController(IInvoiceService invoiceService) : ControllerBase
{
    private readonly IInvoiceService _invoiceService = invoiceService;

    // Mock data for development/demo purposes
    private readonly List<InvoiceDto> _mockInvoices = new()
    {
        new InvoiceDto
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            InvoiceNumber = "INV1981",
            EventId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            EventName = "Echo Beats Festival",
            UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            UserName = "Jackson Moore",
            Amount = 654m,
            IssueDate = new DateTime(2025, 5, 20),
            DueDate = new DateTime(2025, 6, 15),
            Status = "Overdue",
            Description = "Event ticket payment",
            CreatedAt = new DateTime(2025, 4, 28),
        },
        new InvoiceDto
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            InvoiceNumber = "INV1987",
            EventId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            EventName = "Runway Revolution 2029",
            UserId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            UserName = "Alicia Smithson",
            Amount = 5656m,
            IssueDate = new DateTime(2025, 8, 17),
            DueDate = new DateTime(2025, 9, 15),
            Status = "Overdue",
            Description = "VIP event package",
            CreatedAt = new DateTime(2025, 8, 17),
        },
        new InvoiceDto
        {
            Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
            InvoiceNumber = "INV888",
            EventId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
            EventName = "Symphony Under the Stars",
            UserId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            UserName = "Patrick Cooper",
            Amount = 5500m,
            IssueDate = new DateTime(2025, 3, 2),
            DueDate = new DateTime(2025, 4, 1),
            Status = "Overdue",
            Description = "Premium seating reservation",
            CreatedAt = new DateTime(2025, 3, 2),
        },
        new InvoiceDto
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            InvoiceNumber = "INV2001",
            EventId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            EventName = "Tech Future Expo",
            UserId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            UserName = "Sarah Chen",
            Amount = 1200m,
            IssueDate = new DateTime(2025, 1, 15),
            DueDate = new DateTime(2025, 2, 15),
            Status = "Paid",
            Description = "Conference registration",
            CreatedAt = new DateTime(2025, 1, 15),
        },
        new InvoiceDto
        {
            Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            InvoiceNumber = "INV2002",
            EventId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            EventName = "Culinary Delights Festival",
            UserId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
            UserName = "Clara Simmons",
            Amount = 850m,
            IssueDate = new DateTime(2025, 2, 10),
            DueDate = new DateTime(2025, 3, 10),
            Status = "Pending",
            Description = "Food festival tickets",
            CreatedAt = new DateTime(2025, 2, 10),
        },
    };

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            await Task.Delay(100); // Simulate async operation
            return Ok(new { Success = true, Result = _mockInvoices });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            await Task.Delay(50); // Simulate async operation
            var invoice = _mockInvoices.FirstOrDefault(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound(new { Success = false, Error = "Invoice not found" });
            }

            return Ok(new { Success = true, Result = invoice });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    [HttpGet("number/{invoiceNumber}")]
    public async Task<IActionResult> GetByNumber(string invoiceNumber)
    {
        try
        {
            await Task.Delay(50); // Simulate async operation
            var invoice = _mockInvoices.FirstOrDefault(i => i.InvoiceNumber == invoiceNumber);

            if (invoice == null)
            {
                return NotFound(new { Success = false, Error = "Invoice not found" });
            }

            return Ok(new { Success = true, Result = invoice });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        try
        {
            await Task.Delay(100); // Simulate async operation
            var invoices = _mockInvoices.Where(i =>
                i.Status.Equals(status, StringComparison.OrdinalIgnoreCase)
            );

            return Ok(new { Success = true, Result = invoices });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdue()
    {
        try
        {
            await Task.Delay(100); // Simulate async operation
            var overdueInvoices = _mockInvoices.Where(i =>
                i.Status.Equals("Overdue", StringComparison.OrdinalIgnoreCase)
                || (
                    i.DueDate < DateTime.Now
                    && !i.Status.Equals("Paid", StringComparison.OrdinalIgnoreCase)
                )
            );

            return Ok(new { Success = true, Result = overdueInvoices });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetByEvent(Guid eventId)
    {
        try
        {
            await Task.Delay(100); // Simulate async operation
            var invoices = _mockInvoices.Where(i => i.EventId == eventId);

            return Ok(new { Success = true, Result = invoices });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(Guid userId)
    {
        try
        {
            await Task.Delay(100); // Simulate async operation
            var invoices = _mockInvoices.Where(i => i.UserId == userId);

            return Ok(new { Success = true, Result = invoices });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvoiceDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Success = false, Error = "Invalid data" });
            }

            await Task.Delay(200); // Simulate async operation

            var newInvoice = new InvoiceDto
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = $"INV{DateTime.Now:yyyyMMddHHmmss}",
                EventId = request.EventId,
                EventName = "New Event", // In real implementation, fetch from EventService
                UserId = request.UserId,
                UserName = "New User", // In real implementation, fetch from UserService
                Amount = request.Amount,
                IssueDate = DateTime.Now,
                DueDate = request.DueDate,
                Status = "Pending",
                Description = request.Description,
                CreatedAt = DateTime.Now,
            };

            return Ok(new { Success = true, Result = newInvoice });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateInvoiceDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Success = false, Error = "Invalid data" });
            }

            await Task.Delay(200); // Simulate async operation

            var existingInvoice = _mockInvoices.FirstOrDefault(i => i.Id == id);
            if (existingInvoice == null)
            {
                return NotFound(new { Success = false, Error = "Invoice not found" });
            }

            // Update the invoice (in real implementation, this would update the database)
            var updatedInvoice = new InvoiceDto
            {
                Id = existingInvoice.Id,
                InvoiceNumber = existingInvoice.InvoiceNumber,
                EventId = existingInvoice.EventId,
                EventName = existingInvoice.EventName,
                UserId = existingInvoice.UserId,
                UserName = existingInvoice.UserName,
                Amount = request.Amount,
                IssueDate = existingInvoice.IssueDate,
                DueDate = request.DueDate,
                Status = request.Status ?? existingInvoice.Status,
                Description = request.Description ?? existingInvoice.Description,
                CreatedAt = existingInvoice.CreatedAt,
            };

            return Ok(new { Success = true, Result = updatedInvoice });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await Task.Delay(100); // Simulate async operation

            var invoice = _mockInvoices.FirstOrDefault(i => i.Id == id);
            if (invoice == null)
            {
                return NotFound(new { Success = false, Error = "Invoice not found" });
            }

            return Ok(new { Success = true, Message = "Invoice deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Error = ex.Message });
        }
    }
}

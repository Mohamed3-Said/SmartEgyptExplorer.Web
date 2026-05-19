using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Services;
using ServiceAbstraction.Services.IMobileService;
using Shared.DTOS.BookingDTOs;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // POST api/tickets/book
        [HttpPost("book")]
        public async Task<IActionResult> Book([FromBody] BookTicketDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _ticketService.BookAsync(userId, dto);
            return Ok(result);
        }

        // GET api/tickets/my-tickets
        [HttpGet("my-tickets")]
        public async Task<IActionResult> GetMyTickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _ticketService.GetMyTicketsAsync(userId);
            return Ok(result);
        }

        // GET api/tickets/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _ticketService.GetTicketByIdAsync(userId, id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}

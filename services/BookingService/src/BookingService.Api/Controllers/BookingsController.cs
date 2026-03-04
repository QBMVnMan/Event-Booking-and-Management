using Microsoft.AspNetCore.Mvc;

namespace BookingService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private static readonly List<BookingItem> Bookings = new()
        {
            new BookingItem { Id = "1", EventId = "1", UserId = "1", Quantity = 2, Note = "Vé VIP" },
            new BookingItem { Id = "2", EventId = "2", UserId = "2", Quantity = 1, Note = "Vé thường" },
            new BookingItem { Id = "3", EventId = "3", UserId = "3", Quantity = 3, Note = "Đặt nhóm" },
            new BookingItem { Id = "4", EventId = "4", UserId = "4", Quantity = 1, Note = "Online" }
        };

        [HttpGet]
        public IActionResult Get() => Ok(Bookings);

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var booking = Bookings.FirstOrDefault(b => b.Id == id);
            return booking == null ? NotFound() : Ok(booking);
        }
    }

    public class BookingItem
    {
        public string Id { get; set; } = string.Empty;
        public string EventId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? Note { get; set; }
    }
}

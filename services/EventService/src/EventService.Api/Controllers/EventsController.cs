using Microsoft.AspNetCore.Mvc;

namespace EventService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {

        private static readonly List<EventItem> Events = new()
        {
            new EventItem { Id = "1", Name = "Nhạc sống: Live Concert", Category = "Nhạc sống", Date = DateTime.UtcNow.AddDays(5), Venue = "Hà Nội", Poster = "", MinPrice = 100000 },
            new EventItem { Id = "2", Name = "Hội thảo & Workshop: Kỹ năng mềm", Category = "Hội thảo & Workshop", Date = DateTime.UtcNow.AddDays(10), Venue = "Hồ Chí Minh", Poster = "", MinPrice = 150000 },
            new EventItem { Id = "3", Name = "Thể thao: Bóng đá", Category = "Thể thao", Date = DateTime.UtcNow.AddDays(15), Venue = "Đà Nẵng", Poster = "", MinPrice = 200000 },
            new EventItem { Id = "4", Name = "Phim & Điện ảnh: Premiere", Category = "Phim & Điện ảnh", Date = DateTime.UtcNow.AddDays(20), Venue = "Huế", Poster = "", MinPrice = 80000 },
            new EventItem { Id = "5", Name = "Kịch & Nghệ thuật: Sân khấu", Category = "Kịch & Nghệ thuật", Date = DateTime.UtcNow.AddDays(25), Venue = "Cần Thơ", Poster = "", MinPrice = 120000 },
            new EventItem { Id = "6", Name = "Voucher & Khác: Ưu đãi", Category = "Voucher & Khác", Date = DateTime.UtcNow.AddDays(30), Venue = "Online", Poster = "", MinPrice = 50000 }
        };

        [HttpGet]
        public IActionResult Get([FromQuery] string? category = null, [FromQuery] string? q = null)
        {
            var result = Events.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(category))
                result = result.Where(e =>
                    (e.Category != null && e.Category.Contains(category, StringComparison.OrdinalIgnoreCase)) ||
                    (e.Name != null && e.Name.Contains(category, StringComparison.OrdinalIgnoreCase))
                );
            if (!string.IsNullOrWhiteSpace(q))
                result = result.Where(e => e.Name != null && e.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
            return Ok(result);
        }

        [HttpGet("featured")]
        public IActionResult GetFeatured()
        {
            // For demo, just return the first 2 events as featured
            return Ok(Events.Take(2));
        }
    }

    public class EventItem
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public DateTime Date { get; set; }
        public string? Venue { get; set; }
        public string? Poster { get; set; }
        public int? MinPrice { get; set; }
    }
}

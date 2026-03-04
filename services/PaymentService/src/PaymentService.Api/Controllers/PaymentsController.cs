using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private static readonly List<PaymentItem> Payments = new()
        {
            new PaymentItem { Id = "1", BookingId = "1", Amount = 200000, Status = "Hoàn tất", Note = "Thanh toán qua Momo" },
            new PaymentItem { Id = "2", BookingId = "2", Amount = 100000, Status = "Chờ xử lý", Note = "Chuyển khoản" },
            new PaymentItem { Id = "3", BookingId = "3", Amount = 150000, Status = "Hoàn tất", Note = "Tiền mặt" },
            new PaymentItem { Id = "4", BookingId = "4", Amount = 50000, Status = "Thất bại", Note = "Lỗi ngân hàng" }
        };

        [HttpGet]
        public IActionResult Get() => Ok(Payments);

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var payment = Payments.FirstOrDefault(p => p.Id == id);
            return payment == null ? NotFound() : Ok(payment);
        }
    }

    public class PaymentItem
    {
        public string Id { get; set; } = string.Empty;
        public string BookingId { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}

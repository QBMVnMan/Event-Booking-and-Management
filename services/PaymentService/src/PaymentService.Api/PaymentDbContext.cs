using Microsoft.EntityFrameworkCore;

namespace PaymentService.Api;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

    public DbSet<PaymentItem> Payments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<PaymentItem>().HasData(
            new PaymentItem { Id = "1", BookingId = "1", Amount = 200000, Status = "Hoàn tất", Note = "Thanh toán qua Momo" },
            new PaymentItem { Id = "2", BookingId = "2", Amount = 100000, Status = "Chờ xử lý", Note = "Chuyển khoản" },
            new PaymentItem { Id = "3", BookingId = "3", Amount = 150000, Status = "Hoàn tất", Note = "Tiền mặt" },
            new PaymentItem { Id = "4", BookingId = "4", Amount = 50000, Status = "Thất bại", Note = "Lỗi ngân hàng" }
        );
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
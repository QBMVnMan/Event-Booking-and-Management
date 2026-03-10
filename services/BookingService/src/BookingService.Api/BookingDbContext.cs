using Microsoft.EntityFrameworkCore;

namespace BookingService.Api;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

    public DbSet<BookingItem> Bookings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<BookingItem>().HasData(
            new BookingItem { Id = "1", EventId = "1", UserId = "1", Quantity = 2, Note = "Vé VIP" },
            new BookingItem { Id = "2", EventId = "2", UserId = "2", Quantity = 1, Note = "Vé thường" },
            new BookingItem { Id = "3", EventId = "3", UserId = "3", Quantity = 3, Note = "Đặt nhóm" },
            new BookingItem { Id = "4", EventId = "4", UserId = "4", Quantity = 1, Note = "Online" }
        );
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
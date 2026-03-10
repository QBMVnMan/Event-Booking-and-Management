using Microsoft.EntityFrameworkCore;

namespace EventService.Api;

public class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

    public DbSet<EventItem> Events { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<EventItem>().HasData(
            new EventItem { Id = "1", Name = "Nhạc sống: Live Concert", Category = "Nhạc sống", Date = DateTime.Parse("2026-03-09T07:53:41.6080015Z"), Venue = "Hà Nội", Poster = "", MinPrice = 100000 },
            new EventItem { Id = "2", Name = "Hội thảo & Workshop: Kỹ năng mềm", Category = "Hội thảo & Workshop", Date = DateTime.Parse("2026-03-14T07:53:41.6082118Z"), Venue = "Hồ Chí Minh", Poster = "", MinPrice = 150000 },
            new EventItem { Id = "3", Name = "Thể thao: Bóng đá", Category = "Thể thao", Date = DateTime.Parse("2026-03-19T07:53:41.6082127Z"), Venue = "Đà Nẵng", Poster = "", MinPrice = 200000 },
            new EventItem { Id = "4", Name = "Phim & Điện ảnh: Premiere", Category = "Phim & Điện ảnh", Date = DateTime.Parse("2026-03-24T07:53:41.6082129Z"), Venue = "Huế", Poster = "", MinPrice = 80000 },
            new EventItem { Id = "5", Name = "Kịch & Nghệ thuật: Sân khấu", Category = "Kịch & Nghệ thuật", Date = DateTime.Parse("2026-03-29T07:53:41.6082132Z"), Venue = "Cần Thơ", Poster = "", MinPrice = 120000 },
            new EventItem { Id = "6", Name = "Voucher & Khác: Ưu đãi", Category = "Voucher & Khác", Date = DateTime.Parse("2026-04-03T07:53:41.6082148Z"), Venue = "Online", Poster = "", MinPrice = 50000 }
        );
    }
}

public class EventItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Venue { get; set; } = string.Empty;
    public string Poster { get; set; } = string.Empty;
    public decimal MinPrice { get; set; }
}
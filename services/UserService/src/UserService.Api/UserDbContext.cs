using Microsoft.EntityFrameworkCore;

namespace UserService.Api;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<UserItem> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<UserItem>().HasData(
            new UserItem { Id = "1", Username = "alice", Email = "alice@example.com", FullName = "Nguyễn Thị Alice" },
            new UserItem { Id = "2", Username = "bob", Email = "bob@example.com", FullName = "Trần Văn Bob" },
            new UserItem { Id = "3", Username = "linh", Email = "linh@example.com", FullName = "Lê Thị Linh" },
            new UserItem { Id = "4", Username = "quang", Email = "quang@example.com", FullName = "Phạm Quang" }
        );
    }
}

public class UserItem
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }
}
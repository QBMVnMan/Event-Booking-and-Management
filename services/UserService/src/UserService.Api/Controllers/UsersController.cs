using Microsoft.AspNetCore.Mvc;

namespace UserService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<UserItem> Users = new()
        {
            new UserItem { Id = "1", Username = "alice", Email = "alice@example.com", FullName = "Nguyễn Thị Alice" },
            new UserItem { Id = "2", Username = "bob", Email = "bob@example.com", FullName = "Trần Văn Bob" },
            new UserItem { Id = "3", Username = "linh", Email = "linh@example.com", FullName = "Lê Thị Linh" },
            new UserItem { Id = "4", Username = "quang", Email = "quang@example.com", FullName = "Phạm Quang" }
        };

        [HttpGet]
        public IActionResult Get() => Ok(Users);

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            return user == null ? NotFound() : Ok(user);
        }
    }

    public class UserItem
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
    }
}

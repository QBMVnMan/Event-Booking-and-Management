using Contracts;

namespace UserService.Api.Services;

public class InMemoryUserService : IUserService
{
    private readonly Dictionary<string, (Guid Id, string PasswordHash)> _users = new(StringComparer.OrdinalIgnoreCase);

    public Task<UserDto> RegisterAsync(RegisterRequest request)
    {
        if (_users.ContainsKey(request.Username))
            throw new InvalidOperationException("User already exists");

        var id = Guid.NewGuid();
        // NOTE: This is a demo. Use a proper password hasher (PBKDF2/Argon2) in production.
        var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(request.Password));
        _users[request.Username] = (id, passwordHash);
        return Task.FromResult(new UserDto(id, request.Username));
    }

    public Task<UserDto?> GetByUsernameAsync(string username)
    {
        if (_users.TryGetValue(username, out var v))
            return Task.FromResult<UserDto?>(new UserDto(v.Id, username));
        return Task.FromResult<UserDto?>(null);
    }

    public Task<UserDto?> ValidateCredentialsAsync(string username, string password)
    {
        if (_users.TryGetValue(username, out var v))
        {
            var hash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
            if (hash == v.PasswordHash)
                return Task.FromResult<UserDto?>(new UserDto(v.Id, username));
        }
        return Task.FromResult<UserDto?>(null);
    }
}
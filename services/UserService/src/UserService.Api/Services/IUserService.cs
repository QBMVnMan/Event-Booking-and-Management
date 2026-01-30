using Contracts;

namespace UserService.Api.Services;

public interface IUserService
{
    Task<UserDto> RegisterAsync(RegisterRequest request);
    Task<UserDto?> GetByUsernameAsync(string username);
    Task<UserDto?> ValidateCredentialsAsync(string username, string password);
}
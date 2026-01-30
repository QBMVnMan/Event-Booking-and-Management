namespace Contracts;

public record RegisterRequest(string Username, string Password);
public record LoginRequest(string Username, string Password);
public record LoginResponse(string AccessToken, string TokenType = "Bearer");
public record UserDto(Guid Id, string Username);

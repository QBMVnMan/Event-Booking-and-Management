using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserService.Api.Services;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthController(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            var user = await _userService.RegisterAsync(request);
            return CreatedAtAction(null, user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userService.ValidateCredentialsAsync(request.Username, request.Password);
        if (user is null)
            return Unauthorized();

        var token = GenerateJwt(user);
        return Ok(new LoginResponse(token));
    }

    private string GenerateJwt(UserDto user)
    {
        var key = _configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT:Key is not configured");
        var issuer = _configuration["JWT:Issuer"] ?? "EventBooking";
        var audience = _configuration["JWT:Audience"] ?? "EventBooking";

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
        };

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
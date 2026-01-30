using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// CORS - only allow local frontend by default
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

// Register in-memory user store
builder.Services.AddSingleton<IUserService, InMemoryUserService>();

// Authentication
var jwtKey = builder.Configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT:Key is not configured");
var issuer = builder.Configuration["JWT:Issuer"] ?? "EventBooking";
var audience = builder.Configuration["JWT:Audience"] ?? "EventBooking";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // dev only
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("frontend");
app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/api/auth/register", (Contracts.RegisterRequest request, IUserService users) =>
{
    try
    {
        var u = users.RegisterAsync(request).GetAwaiter().GetResult();
        return Results.Created($"/api/users/{u.Id}", u);
    }
    catch (InvalidOperationException)
    {
        return Results.Conflict();
    }
});

app.MapPost("/api/auth/login", async (Contracts.LoginRequest request, IUserService users) =>
{
    var user = await users.ValidateCredentialsAsync(request.Username, request.Password);
    if (user is null) return Results.Unauthorized();

    // Temporary token generation for non-controller endpoints
    var jwtKeyLocal = builder.Configuration["JWT:Key"]!;
    var issuerLocal = builder.Configuration["JWT:Issuer"] ?? "EventBooking";
    var audienceLocal = builder.Configuration["JWT:Audience"] ?? "EventBooking";

    var claims = new[]
    {
        new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName, user.Username)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKeyLocal));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(issuerLocal, audienceLocal, claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: creds);
    var tokenStr = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(new Contracts.LoginResponse(tokenStr));
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

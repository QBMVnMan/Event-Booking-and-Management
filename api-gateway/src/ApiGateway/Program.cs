using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Http client for simple proxy forwarding
builder.Services.AddHttpClient("gatewayProxy");

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

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

// Simple development proxy: forward requests under /api/{service} to local services.
// This keeps the gateway lightweight and allows the frontend to call the gateway
// while developers run services on localhost ports (5001..5004).
var httpClientFactory = app.Services.GetRequiredService<IHttpClientFactory>();
var proxyClient = httpClientFactory.CreateClient("gatewayProxy");

var serviceMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
{
    ["events"] = 5001,
    ["users"] = 5002,
    ["bookings"] = 5003,
    ["payments"] = 5004
};

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/api"), appBuilder =>
{
    appBuilder.Run(async ctx =>
    {
        var path = ctx.Request.Path.Value ?? string.Empty; // e.g. /api/events/featured
        var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            ctx.Response.StatusCode = 404;
            await ctx.Response.WriteAsync("Not Found");
            return;
        }

        var serviceName = parts[1];
        if (!serviceMap.TryGetValue(serviceName, out var port))
        {
            ctx.Response.StatusCode = 502;
            await ctx.Response.WriteAsync($"Unknown service: {serviceName}");
            return;
        }

        // Build target URI
        var rest = parts.Length > 2 ? string.Join('/', parts.Skip(2)) : string.Empty;
        var target = new UriBuilder
        {
            Scheme = "http",
            Host = "localhost",
            Port = port,
            Path = $"/api/{serviceName}" + (string.IsNullOrEmpty(rest) ? string.Empty : "/" + rest),
            Query = ctx.Request.QueryString.HasValue ? ctx.Request.QueryString.Value!.TrimStart('?') : string.Empty
        }.Uri;

        using var requestMessage = new HttpRequestMessage(new HttpMethod(ctx.Request.Method), target);

        // Copy headers
        foreach (var header in ctx.Request.Headers)
        {
            // Skip host header
            if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
            {
                requestMessage.Content ??= new StreamContent(Stream.Null);
                requestMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }

        // Copy body if present
        if (ctx.Request.ContentLength > 0)
        {
            requestMessage.Content = new StreamContent(ctx.Request.Body);
        }

        HttpResponseMessage? forwardedResponse = null;
        try
        {
            forwardedResponse = await proxyClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, ctx.RequestAborted);
        }
        catch (Exception ex)
        {
            ctx.Response.StatusCode = 502;
            await ctx.Response.WriteAsync("Error forwarding request: " + ex.Message);
            return;
        }

        ctx.Response.StatusCode = (int)forwardedResponse.StatusCode;
        foreach (var header in forwardedResponse.Headers)
        {
            ctx.Response.Headers[header.Key] = header.Value.ToArray();
        }
        if (forwardedResponse.Content != null)
        {
            foreach (var header in forwardedResponse.Content.Headers)
            {
                ctx.Response.Headers[header.Key] = header.Value.ToArray();
            }
            // Remove transfer-encoding if present as it may cause issues
            ctx.Response.Headers.Remove("transfer-encoding");
            await forwardedResponse.Content.CopyToAsync(ctx.Response.Body);
        }
    });
});

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

app.MapGet("/api/protected", (ClaimsPrincipal user) =>
{
    return Results.Ok(new { message = "This is protected", user = user.Identity?.Name });
}).RequireAuthorization();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

using RetroVault.Server.Models;
using RetroVault.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var user = new RetroVaultUser
{
    Email = "test@example.com",
    Username = "TestPlayer",
    PasswordHash = "...",
    PasswordSalt = "...",
    Profile = new() { DisplayName = "Test Player" },
    Settings = new() { Theme = Themes.Dark }
};

var service = new CosmosService();
await service.AddUserAsync(user);

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

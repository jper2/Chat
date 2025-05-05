using Chat.Core.Middleware;
using Chat.Core.Models;
using Chat.Core.Repositories;
using Chat.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==================================================
// 1. Configure Logging
// ==================================================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 20_971_520, rollOnFileSizeLimit: true, retainedFileCountLimit: 30)
    .CreateLogger();

builder.Host.UseSerilog();
Log.Information("Starting Chat API...");

// ==================================================
// 2. Configure Services
// ==================================================

// Add Controllers
builder.Services.AddControllers();

// Add OpenAPI/Swagger for API documentation
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<IMongoDbSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Add SignalR for real-time communication
builder.Services.AddSignalR();

// Register Repositories
builder.Services.AddScoped<IMessageRepository, MongoDBMessageRepository>();
builder.Services.AddScoped<IUsersRepository, MongoDBUsersRepository>();

// Register Services
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IMessageService, MessageService>();

// Configure JWT Authentication
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key) || string.IsNullOrEmpty(jwtSettings.Issuer) || string.IsNullOrEmpty(jwtSettings.Audience))
{
    throw new InvalidOperationException("JWT settings are not configured properly in the appsettings.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Replace with your frontend's URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// ==================================================
// 3. Build the Application
// ==================================================
var app = builder.Build();

// ==================================================
// 4. Configure Middleware
// ==================================================

// Enable OpenAPI/Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enable CORS
app.UseCors("AllowSpecificOrigin");

// Add custom error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Add routing middleware
app.UseRouting();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// ==================================================
// 5. Map Endpoints
// ==================================================

// Map SignalR hubs
app.MapHub<Chat.Core.Hubs.ChatHub>("/hubs/chat");

// Map API controllers
app.MapControllers();

// ==================================================
// 6. Run the Application
// ==================================================
app.Run();

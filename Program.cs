using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PersonalDetailsAPI.Data;
using PersonalDetailsAPI.Middleware;
using PersonalDetailsAPI.Repositories;
using PersonalDetailsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// ? ADD THIS: Configure port for Railway deployment
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container
builder.Services.AddControllers();

// Configure MySQL Database
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

// Parse Railway's mysql:// format
if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("mysql://"))
{
    try
    {
        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo.Split(':');
        connectionString = $"Server={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};User={userInfo[0]};Password={userInfo[1]};";
        Console.WriteLine("✅ DATABASE_URL parsed");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Parse error: {ex.Message}");
    }
}

if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}


var serverVersion = ServerVersion.AutoDetect(connectionString);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, serverVersion));
// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Register Repositories
builder.Services.AddScoped<IPersonalDetailRepository, PersonalDetailRepository>();

// Register Services
builder.Services.AddScoped<IPersonalDetailService, PersonalDetailService>();

// Configure CORS
var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?.Split(',')
    ?? new[] { "*" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        if (allowedOrigins.Contains("*"))
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
    });
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Personal Details API",
        Version = "v1",
        Description = "API for managing personal details with family information",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@example.com"
        }
    });

    // Enable XML comments for Swagger documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();


// Test database connection
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.CanConnectAsync();
    Console.WriteLine("✅ DB connected");
    await db.Database.MigrateAsync();
    Console.WriteLine("✅ Migrations applied");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ DB error: {ex.Message}");
}

// Configure the HTTP request pipeline
// Enable Swagger in all environments (for Railway deployment)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Personal Details API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at root
});

// Use custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();

//app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();

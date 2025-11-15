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
string connectionString = "";
string host = "localhost";
string port2 = "3306";
string database = "railway";
string user = "root";
string password = "";

// Try to parse DATABASE_URL first (Railway's preferred format)
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(databaseUrl))
{
    Console.WriteLine("🔍 Using DATABASE_URL for connection");
    try
    {
        // Parse DATABASE_URL format: mysql://user:password@host:port/database
        var uri = new Uri(databaseUrl);
        host = uri.Host;
        port2 = uri.Port > 0 ? uri.Port.ToString() : "3306";
        database = uri.AbsolutePath.TrimStart('/');

        // Parse user:password from UserInfo
        if (!string.IsNullOrEmpty(uri.UserInfo) && uri.UserInfo.Contains(':'))
        {
            var userInfo = uri.UserInfo.Split(':');
            user = userInfo[0];
            password = userInfo.Length > 1 ? userInfo[1] : "";
        }
        else
        {
            user = uri.UserInfo ?? "root";
            password = "";
        }

        connectionString = $"Server={host};Port={port2};Database={database};User={user};Password={password};";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Failed to parse DATABASE_URL: {ex.Message}");
        Console.WriteLine("Falling back to individual environment variables...");
        databaseUrl = null; // Trigger fallback
    }
}

// Fallback to individual environment variables (for local development or if DATABASE_URL fails)
if (string.IsNullOrEmpty(databaseUrl))
{
    Console.WriteLine("🔍 Using individual environment variables for connection");

    // Support multiple variable name formats (MYSQLHOST, MYSQL_HOST, etc.)
    host = Environment.GetEnvironmentVariable("MYSQLHOST")
        ?? Environment.GetEnvironmentVariable("MYSQL_HOST")
        ?? "localhost";

    port2 = Environment.GetEnvironmentVariable("MYSQLPORT")
        ?? Environment.GetEnvironmentVariable("MYSQL_PORT")
        ?? "3306";

    database = Environment.GetEnvironmentVariable("MYSQLDATABASE")
        ?? Environment.GetEnvironmentVariable("MYSQL_DATABASE")
        ?? "railway";

    user = Environment.GetEnvironmentVariable("MYSQLUSER")
        ?? Environment.GetEnvironmentVariable("MYSQL_USER")
        ?? "root";

    password = Environment.GetEnvironmentVariable("MYSQLPASSWORD")
        ?? Environment.GetEnvironmentVariable("MYSQL_PASSWORD")
        ?? "";

    connectionString = $"Server={host};Port={port2};Database={database};User={user};Password={password};";
}

// Log connection details (mask password for security)
Console.WriteLine($"🔍 Connection Details:");
Console.WriteLine($"  Host: {host}");
Console.WriteLine($"  Port: {port2}");
Console.WriteLine($"  Database: {database}");
Console.WriteLine($"  User: {user}");
Console.WriteLine($"  Password: {(string.IsNullOrEmpty(password) ? "[EMPTY - WARNING!]" : "[SET]")}");

// Validate credentials before attempting connection
if (string.IsNullOrEmpty(user))
{
    Console.WriteLine("❌ ERROR: Database user is empty!");
    Console.WriteLine("Please check your Railway environment variables.");
    Console.WriteLine("Available environment variables:");
    foreach (var envVar in Environment.GetEnvironmentVariables().Keys.Cast<string>().Where(k => k.Contains("MYSQL") || k.Contains("DATABASE")))
    {
        Console.WriteLine($"  - {envVar}");
    }
}

if (string.IsNullOrEmpty(password))
{
    Console.WriteLine("⚠️ WARNING: Database password is empty! This may cause authentication failures.");
}

// Final fallback to appsettings.json if still empty
if (string.IsNullOrEmpty(connectionString) || connectionString.Length < 20)
{
    Console.WriteLine("⚠️ Connection string appears invalid, trying appsettings.json...");
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
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

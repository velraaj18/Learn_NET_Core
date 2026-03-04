using FirstApi.Middleware;
using FirstAPI.Data;
using FirstAPI.DTO;
using FirstAPI.Mappings;
using FirstAPI.Models;
using FirstAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();

// Add services for dependency injection
// Everytime we add an service which might be used in many places we need to register them instead of creating new instances in every place.
builder.Services.AddScoped<WeatherService>();
builder.Services.AddScoped<AuthService>();

// Built in password hasher
builder.Services.AddScoped<PasswordHasher<User>>();

// Register DB context
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Auto mapper profiles
// Any class that inherits from AutoMapper's "Profile" class it will be registered.
builder.Services.AddAutoMapper(typeof(Program));

// This is to get the app settings strongly typed and use it controllers by injecting <IOptions>
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// JWT settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// For JWT, first we need to configure "Add Authentication" and then "Add Authorization"
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
    };
});

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

// Add Use Authentication and Use Authorization to validate the JWT token (the configuration we added before [AddAuthentication]) before it reaches the API controllers.
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();



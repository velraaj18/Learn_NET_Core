using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FirstAPI.Data;
using FirstAPI.DTO;
using FirstAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FirstAPI.Services;

public class AuthService
{
    private readonly AppDBContext _dbContext;
    private readonly PasswordHasher<User> _passwordHasher;
    // Strongly typed app setting registered in program.cs
    private readonly JwtSettings _jwtSettings;

    public AuthService(AppDBContext dbContext, PasswordHasher<User> passwordHasher, IOptions<JwtSettings> jwtSettings)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<APIResponse<User>> Register(UserRequest dto)
    {
        if(dto == null)
        {
            return new APIResponse<User>{StatusCode = 400, Message = "You must provide the details", Data = null};
        }

        var existingUser = _dbContext.Users.Where(x=> x.Email == dto.Email).FirstOrDefault();
        if(existingUser != null)
        {
            return new APIResponse<User>{StatusCode = 400, Message = "User Already registered", Data = existingUser};
        }

        // Create a new User entity
        var user = new User
        {
            Email = dto.Email
        };

        // Hash the plain password
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return new APIResponse<User>
        {
            StatusCode = 201,
            Message = "User registered successfully",
            Data = user
        };
    }

    public async Task<APIResponse<dynamic>> Login(UserRequest dto)
    {
        if(dto == null)
        {
            return new APIResponse<dynamic>{StatusCode = 400, Message = "You must provide the details", Data = null};
        }

        var user = _dbContext.Users.Where(x=> x.Email == dto.Email).FirstOrDefault();
        if(user == null)
        {
            return new APIResponse<dynamic>{StatusCode = 401, Message = "Invalid Email or Password", Data = null};
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if(result != PasswordVerificationResult.Success)
        {
            return new APIResponse<dynamic>{StatusCode = 401, Message = "Invalid Email or Password", Data = null};
        }

        // --------------- JWT token generation ------------------ //

        // Create claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserUID.ToString()),
            new(ClaimTypes.Email, user.Email)
        };

        // Create signing key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        // Create signing credentials
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create token
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims : claims,
            expires : DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: cred
        );

        // Convert token to string
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new APIResponse<dynamic>{StatusCode = 200, Message = "User logged in successfully", Data = new {token = tokenString}};
    }
}

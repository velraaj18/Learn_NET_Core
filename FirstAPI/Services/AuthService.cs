using System;
using FirstAPI.Data;
using FirstAPI.DTO;
using FirstAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace FirstAPI.Services;

public class AuthService
{
    private readonly AppDBContext _dbContext;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(AppDBContext dbContext, PasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
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

    public async Task<APIResponse<User>> Login(UserRequest dto)
    {
        if(dto == null)
        {
            return new APIResponse<User>{StatusCode = 400, Message = "You must provide the details", Data = null};
        }

        var user = _dbContext.Users.Where(x=> x.Email == dto.Email).FirstOrDefault();
        if(user == null)
        {
            return new APIResponse<User>{StatusCode = 401, Message = "Invalid Email or Password", Data = null};
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if(result != PasswordVerificationResult.Success)
        {
            return new APIResponse<User>{StatusCode = 401, Message = "Invalid Email or Password", Data = null};
        }

        return new APIResponse<User>{StatusCode = 200, Message = "User logged in successfully", Data = user};
    }
}

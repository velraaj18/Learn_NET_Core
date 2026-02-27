using System;
using FirstAPI.Data;
using FirstAPI.DTO;
using FirstAPI.Models;

namespace FirstAPI.Services;

public class AuthService
{
    private readonly AppDBContext _dbContext;

    public AuthService(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> Register(UserRequest dto)
    {
        
    }
}

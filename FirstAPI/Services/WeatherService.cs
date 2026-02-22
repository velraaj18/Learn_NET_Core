using System;
using FirstAPI.Data;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Services;

public class WeatherService
{
    private readonly AppDBContext _dbContext;

    public WeatherService(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    public async Task<List<Weather>> GetAll()
    {
        return await _dbContext.Weathers.ToListAsync();
    }

    public async Task<Weather> Post(Weather weather)
    {
        _dbContext.Weathers.Add(weather);
        await _dbContext.SaveChangesAsync();
        return weather;
    }
}

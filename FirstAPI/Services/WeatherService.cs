using System;
using FirstAPI.Data;
using FirstAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Services;

public class WeatherService
{
    private readonly AppDBContext _dbContext;

    public WeatherService(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    [Authorize]
    public async Task<List<Weather>> GetAll()
    {
        return await _dbContext.Weathers.ToListAsync();
    }

    public async Task<Weather> GetById(int id)
    {
        return await _dbContext.Weathers.FindAsync(id);
    } 

    public async Task<Weather> Post(Weather weather)
    {
        _dbContext.Weathers.Add(weather);
        await _dbContext.SaveChangesAsync();
        return weather;
    }

    public async Task<bool> Update(int id, Weather updatedWeather)
    {
        var response = await _dbContext.Weathers.FindAsync(id);

        if(response == null) return false;

        // update the existing response with the new values
        response.City = updatedWeather.City;
        response.Temperature = updatedWeather.Temperature;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var response = await _dbContext.Weathers.FindAsync(id);

        if(response == null) return false;

        _dbContext.Weathers.Remove(response);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}

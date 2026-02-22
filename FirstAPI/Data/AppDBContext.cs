using System;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {

    }

    public DbSet<Weather> Weathers { get; set; }
}

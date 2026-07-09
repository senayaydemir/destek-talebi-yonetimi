using DestekTalebiYonetimi.Models;
using Microsoft.EntityFrameworkCore;

namespace DestekTalebiYonetimi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<DestekTalebi> DestekTalepleri { get; set; } = null!;
}
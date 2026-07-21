using DestekTalebiYonetimi.Models;
using Microsoft.EntityFrameworkCore;

namespace DestekTalebiYonetimi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
public DbSet<Kullanici> Kullanicilar { get; set; }
    public DbSet<DestekTalebi> DestekTalepleri { get; set; } = null!;
    public DbSet<DestekTalepDosya> DestekTalepDosyalari { get; set; }
    public DbSet<KullaniciLog> KullaniciLoglari { get; set; }
    public DbSet<DestekTalepLog> DestekTalepLoglari { get; set; }
    public DbSet<Bildirim> Bildirimler { get; set; } 
    public DbSet<Mesaj> Mesajlar { get; set; }
}
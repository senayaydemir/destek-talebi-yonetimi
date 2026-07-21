namespace DestekTalebiYonetimi.Models;

public class Bildirim
{
    public int Id { get; set; }

    public string KullaniciAdi { get; set; } = "";
    public string? Rol { get; set; }

    public string Mesaj { get; set; } = "";

    public bool Okundu { get; set; } = false;

    public DateTime Tarih { get; set; } = DateTime.Now;

    public int? DestekTalebiId { get; set; }
}
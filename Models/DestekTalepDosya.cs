namespace DestekTalebiYonetimi.Models;

public class DestekTalepDosya
{
    public int Id { get; set; }

    public int DestekTalebiId { get; set; }

    public string DosyaAdi { get; set; } = "";

    public string DosyaYolu { get; set; } = "";

    public long DosyaBoyutu { get; set; }

    public string DosyaTuru { get; set; } = "";

    public DateTime YuklenmeTarihi { get; set; }

    public DestekTalebi DestekTalebi { get; set; }
}
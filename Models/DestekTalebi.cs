using System;
using System.ComponentModel.DataAnnotations;

namespace DestekTalebiYonetimi.Models;

public class DestekTalebi
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Talep başlığı zorunludur.")]
    public string Baslik { get; set; } = string.Empty;

    [Required(ErrorMessage = "Açıklama zorunludur.")]
    public string Aciklama { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birim seçilmelidir.")]
    public string Birim { get; set; } = string.Empty;

    public string TalepEden { get; set; } = string.Empty;

    public string DahiliNumara { get; set; } = string.Empty;

    public string IlgiliSistem { get; set; } = "EBYS";

    public string TalepTuru { get; set; } = "Sistem Girişi";

    public string Oncelik { get; set; } = "Normal";

    public string Durum { get; set; } = "Bekliyor";

public string? OlusturanKullanici { get; set; }
    public string CozumAciklamasi { get; set; } = string.Empty;

    public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;

    public string? DosyaAdi { get; set; }

public string? DosyaYolu { get; set; }

public long? DosyaBoyutu { get; set; }

public DateTime? DosyaYuklenmeTarihi { get; set; }
}
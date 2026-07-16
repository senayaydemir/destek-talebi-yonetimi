using System.ComponentModel.DataAnnotations;

namespace DestekTalebiYonetimi.Models
{
    public class KullaniciLog
    {
        [Key]
        public int Id { get; set; }

        public string KullaniciAdi { get; set; } = string.Empty;

        public string AdSoyad { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;

        public string Islem { get; set; } = string.Empty;

        public DateTime Tarih { get; set; }

        public string? IpAdresi { get; set; }
        public string? Aciklama { get; set; }

public string? Tarayici { get; set; }
    }
}
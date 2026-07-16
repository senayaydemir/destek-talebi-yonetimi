using System.ComponentModel.DataAnnotations;

namespace DestekTalebiYonetimi.Models
{
    public class DestekTalepLog
    {
        [Key]
        public int Id { get; set; }

        public int DestekTalebiId { get; set; }

        public string KullaniciAdi { get; set; } = string.Empty;

        public string Islem { get; set; } = string.Empty;

        public DateTime Tarih { get; set; }
    }
}
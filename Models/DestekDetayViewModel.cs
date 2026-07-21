using DestekTalebiYonetimi.Models;

namespace DestekTalebiYonetimi.Models
{
    public class DestekDetayViewModel
    {
        public DestekTalebi Talep { get; set; } = null!;

        public List<DestekTalepLog> Loglar { get; set; } = new();

        public List<DestekTalepDosya> Dosyalar { get; set; } = new();
        public List<Mesaj> Mesajlar { get; set; } = new();
    }
}
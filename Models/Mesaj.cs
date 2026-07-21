using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DestekTalebiYonetimi.Models
{
    public class Mesaj
    {
        public int Id { get; set; }

        [Required]
        public int DestekTalebiId { get; set; }

        [ForeignKey("DestekTalebiId")]
        public DestekTalebi DestekTalebi { get; set; }

        [Required]
        public string Gonderen { get; set; } = "";

        [Required]
        public string MesajMetni { get; set; } = "";

        public DateTime Tarih { get; set; } = DateTime.Now;
    }
}
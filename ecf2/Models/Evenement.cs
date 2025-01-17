using System.ComponentModel.DataAnnotations;

namespace ecf2.Models
{
    public class Evenement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        public string Lieu { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}

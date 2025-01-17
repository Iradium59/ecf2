using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ecf2.Models
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        [ForeignKey("Evenement")]
        public int EvenementId { get; set; }
    }
}

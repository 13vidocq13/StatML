using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatML.Models
{
    public class RatingArmy
    {
        [Key]
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public long Rate { get; set; }
        public DateOnly RateDate { get; set; }

        [ForeignKey("PlayerId")]
        public Player? PlayerEntity { get; set; }
    }
}

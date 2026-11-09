using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatML.Models
{
    public class FlagTournamentDetails
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly TournamentDate { get; set; }
        public int PlayerId { get; set; }
        public int? CountOfBasicFlags { get; set; }
        public int? CountOfOtherFlags { get; set; }
        public int? CountOfNotGivenFlags { get; set; }

        [NotMapped]
        public Tournament? TournamentEntity { get; set; }
        public Player? PlayerEntity { get; set; }
    }
}

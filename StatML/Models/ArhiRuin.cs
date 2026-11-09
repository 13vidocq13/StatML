using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatML.Models
{
    public class ArhiRuin
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly DateOnly { get; set; }
        public int PlayerId { get; set; }
        public int? HorizontalPosition { get; set; }
        public int? VerticalPosition { get; set; }
        public int BlackGemCount {get;set;}
        public int? CreatorId { get; set; }
        public int? EditorId { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedId { get; set; }

        public Player? PlayerEntity { get; set; }

        [ForeignKey("CreatorId")]
        public Login? CreatorLoginEntity { get; set; }

        [ForeignKey("EditorId")]
        public Login? EditorLoginEntity { get; set; }
    }
}

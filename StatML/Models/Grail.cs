using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatML.Models;

public partial class Grail
{
    public int Id { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateOnly DateGrail { get; set; }

    public int HorizontalPosition { get; set; }

    public int HoursForPlayer { get; set; }

    public int PlayerId { get; set; }
    public int? CreatorId { get; set; }
    public int? EditorId { get; set; }
    public bool IsDeleted { get; set; }
    public int? DeletedId { get; set; }

    public int VerticalPosition { get; set; }

    [ForeignKey("PlayerId")]
    public Player? PlayerEntity { get; set; }

    [ForeignKey("CreatorId")]
    public Login? CreatorLoginEntity { get; set; }

    [ForeignKey("EditorId")]
    public Login? EditorLoginEntity { get; set; }
}

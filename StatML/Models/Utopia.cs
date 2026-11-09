using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatML.Models;

public partial class Utopia
{
    public int Id { get; set; }

    public int ArtifactLevel1 { get; set; }

    public int ArtifactLevel2 { get; set; }

    public int ArtifactLevel3 { get; set; }

    public int ArtifactLevel4 { get; set; }

    public int ArtifactLevel5 { get; set; }

    public int ArtifactLevel6 { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly DateOnly { get; set; }

    public int PlayerId1 { get; set; }

    public int PlayerId2 { get; set; }
    public int? CreatorId { get; set; }
    public int? EditorId { get; set; }
    public bool IsDeleted { get; set; }
    public int? DeletedId { get; set; }

    public Player? PlayerEntity1 { get; set; }

    public Player? PlayerEntity2 { get; set; }

    [ForeignKey("CreatorId")]
    public Login? CreatorLoginEntity { get; set; }

    [ForeignKey("EditorId")]
    public Login? EditorLoginEntity { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatML.Models;

public partial class Tournament
{
    public int Id { get; set; }

    public int? ArtifactId { get; set; }

    public int? CountOfPoints { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateOnly DateOnly { get; set; }

    public int PlayerId { get; set; }
    public int? CreatorId { get; set; }
    public int? EditorId { get; set; }
    public bool IsDeleted { get; set; }
    public int? DeletedId { get; set; }

    public int PositionInTournament { get; set; }

    public int TournamentTypeId { get; set; }

    public Artifact? ArtifactEntity { get; set; }

    public TournamentType? TournamentTypeEntity { get; set; }

    [ForeignKey("PlayerId")]
    public Player? PlayerEntity { get; set; }

    [ForeignKey("CreatorId")]
    public Login? CreatorLoginEntity { get; set; }

    [ForeignKey("EditorId")]
    public Login? EditorLoginEntity { get; set; }
}

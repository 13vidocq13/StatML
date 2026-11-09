using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatML.Models;

public partial class Player
{
    public int Id { get; set; }

    public string? Clan { get; set; }

    public int? HorizontalPosition { get; set; }

    public bool IsGeneral { get; set; }

    public string PName { get; set; }

    public int? PercentSaltsGrails { get; set; }

    public int? VerticalPosition { get; set; }
    public int? CreatorId { get; set; }
    public int? EditorId { get; set; }
    public bool IsDeleted { get; set; }
    public int? DeletedId { get; set; }

    public IList<FarmZone>? FarmZonesEntities { get; set; }

    public IList<Grail>? GrailEntities { get; set; }

    public IList<Salt>? SaltEntities { get; set; }

    public IList<Tournament>? TournamentsEntities { get; set; }

    public IList<Utopia>? UtopiaPlayerId1 { get; set; }

    public IList<Utopia>? UtopiaPlayerId2 { get; set; }
    public IList<FlagTournamentDetails>? FlagTournamentDetailsEntities { get; set; }
    public IList<ArhiRuin>? ArhiRuinEntities { get; set; }
    public IList<RatingArmy>? RatingArmyEntities { get; set; }

    public Login? LoginEntity { get; set; }

    [ForeignKey("CreatorId")]
    public Login? CreatorLoginEntity { get; set; }

    [ForeignKey("EditorId")]
    public Login? EditorLoginEntity { get; set; }
}

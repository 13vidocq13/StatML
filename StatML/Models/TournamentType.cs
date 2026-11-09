using System;
using System.Collections.Generic;

namespace StatML.Models;

public partial class TournamentType
{
    public int Id { get; set; }

    public string TypeName { get; set; }

    public virtual ICollection<Tournament>? Tournaments { get; set; }
}

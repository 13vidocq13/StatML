using System;
using System.Collections.Generic;

namespace StatML.Models;

public partial class Artifact
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int SetId { get; set; }

    public Sett? SetEntity { get; set; }

    public ICollection<Tournament>? Tournaments { get; set; }
}

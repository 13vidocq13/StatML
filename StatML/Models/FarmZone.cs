using System;
using System.Collections.Generic;

namespace StatML.Models;

public partial class FarmZone
{
    public int Id { get; set; }

    public int HorizontalPosition { get; set; }

    public int PlayerId { get; set; }

    public int VerticalPosition { get; set; }

    public Player? PlayerEntity { get; set; }
}

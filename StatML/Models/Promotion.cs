using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StatML.Models;

public partial class Promotion
{
    public int Id { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly DateOnly { get; set; }

    public int Level { get; set; }
}

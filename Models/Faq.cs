using System;
using System.Collections.Generic;

namespace AptechVisionPetZilla.Models;

public partial class Faq
{
    public int Id { get; set; }

    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;
}

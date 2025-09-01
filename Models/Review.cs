using System;
using System.Collections.Generic;

namespace AptechVisionPetZilla.Models;

public partial class Review
{
    public int Id { get; set; }

    public string? Text { get; set; }

    public string? Name { get; set; }

    public string? Position { get; set; }

    public string? ImageUrl { get; set; }
}

using System;
using System.Collections.Generic;

namespace AptechVisionPetZilla.Models;

public partial class PetCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ActionName { get; set; } = null!;
}

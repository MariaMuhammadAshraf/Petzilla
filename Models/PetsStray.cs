using System;
using System.Collections.Generic;

namespace AptechVisionPetZilla.Models;

public partial class PetsStray
{
    public int PetId { get; set; }

    public string Category { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsAvailable { get; set; }

    public string? ImagePath { get; set; }

    public virtual ICollection<AdoptionRequestsStray> AdoptionRequestsStrays { get; set; } = new List<AdoptionRequestsStray>();
}

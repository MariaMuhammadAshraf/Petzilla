using System;
using System.Collections.Generic;

namespace AptechVisionPetZilla.Models;

public partial class Pet
{
    public int PetId { get; set; }

    public string PetName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int? Age { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsAvailable { get; set; }

    public string? ImagePath { get; set; }

    public virtual ICollection<AdoptionRequestsHome> AdoptionRequestsHomes { get; set; } = new List<AdoptionRequestsHome>();

    public virtual ICollection<PetCareGuideline> PetCareGuidelines { get; set; } = new List<PetCareGuideline>();
}

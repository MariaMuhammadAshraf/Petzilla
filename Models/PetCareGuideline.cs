using System;
using System.Collections.Generic;

namespace AptechVisionPetZilla.Models;

public partial class PetCareGuideline
{
    public int GuidelineId { get; set; }

    public int? PetId { get; set; }

    public string? Food { get; set; }

    public string? Behavior { get; set; }

    public bool? IsKidFriendly { get; set; }

    public string? Precautions { get; set; }

    public virtual Pet? Pet { get; set; }
}

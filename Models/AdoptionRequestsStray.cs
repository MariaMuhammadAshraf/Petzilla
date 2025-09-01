using System;
using System.Collections.Generic;

namespace AptechVisionPetZilla.Models;

public partial class AdoptionRequestsStray
{
    public int RequestId { get; set; }

    public int? PetId { get; set; }

    public string RequesterName { get; set; } = null!;

    public string RequesterEmail { get; set; } = null!;

    public string RequesterPhone { get; set; } = null!;

    public string? RequesterAddress { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public DateTime? RequestedOn { get; set; }

    public virtual PetsStray? Pet { get; set; }
}

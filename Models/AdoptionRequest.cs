using System;
using System.Collections.Generic;

namespace AptechVisionPetZilla.Models;

public partial class AdoptionRequest
{
    public int RequestId { get; set; }

    public int PetId { get; set; }

    public string RequesterName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public DateTime? RequestedOn { get; set; }

    public string? PetType { get; set; }

    public virtual Pet Pet { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AptechVisionPetZilla.Models;

public partial class Ngo
{
    public int NgoId { get; set; }

    [Required(ErrorMessage = "NGO Name is required")]
    public string NgoName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]

    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Phone Number is required")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; } = null!;

    public string? Branches { get; set; }

    public bool AvailabilityStatus { get; set; }

    // Not required, set in controller
    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

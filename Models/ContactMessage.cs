using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AptechVisionPetZilla.Models;

public partial class ContactMessage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;


    [Required(ErrorMessage = "Email is required.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public DateTime? SubmittedAt { get; set; }
}

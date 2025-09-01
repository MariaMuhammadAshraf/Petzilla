using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AptechVisionPetZilla.Models;

public partial class UserRegistration
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "User name is required.")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
    public string UserEmail { get; set; } = null!;


    public string UserPassword { get; set; } = null!;

    public string? UserRole { get; set; }
}

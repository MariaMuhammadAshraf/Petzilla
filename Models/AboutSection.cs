using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AptechVisionPetZilla.Models;

public partial class AboutSection
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    [StringLength(250, ErrorMessage = "Image path cannot exceed 250 characters.")]
    public string? ImagePath { get; set; }

    [StringLength(100, ErrorMessage = "Icon class cannot exceed 100 characters.")]
    public string? IconClass { get; set; }

    [Required(ErrorMessage = "Service Title is required.")]
    [StringLength(100, ErrorMessage = "Service title cannot be longer than 100 characters.")]
    public string? ServiceTitle { get; set; }

    [Required(ErrorMessage = "Service Text is required.")]
    [StringLength(500, ErrorMessage = "Service text cannot exceed 500 characters.")]
    public string? ServiceText { get; set; }
}

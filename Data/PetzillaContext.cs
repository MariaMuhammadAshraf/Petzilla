using System;
using System.Collections.Generic;
using AptechVisionPetZilla.Models;
using Microsoft.EntityFrameworkCore;

namespace AptechVisionPetZilla.Data;

public partial class PetzillaContext : DbContext
{
    public PetzillaContext()
    {
    }

    public PetzillaContext(DbContextOptions<PetzillaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AboutSection> AboutSections { get; set; }

    public virtual DbSet<AdoptionRequestsHome> AdoptionRequestsHomes { get; set; }

    public virtual DbSet<AdoptionRequestsStray> AdoptionRequestsStrays { get; set; }

    public virtual DbSet<ContactMessage> ContactMessages { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    public virtual DbSet<Ngo> Ngos { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<PetCareGuideline> PetCareGuidelines { get; set; }

    public virtual DbSet<PetCategory> PetCategories { get; set; }

    public virtual DbSet<PetsStray> PetsStrays { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<UserRegistration> UserRegistrations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-EUUJM3C\\SQLEXPRESS;Initial Catalog=Petzilla;Persist Security Info=False;User ID=maria;Password=123456;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AboutSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AboutSec__3214EC077FEA13D7");

            entity.Property(e => e.IconClass).HasMaxLength(100);
            entity.Property(e => e.ImagePath).HasMaxLength(300);
            entity.Property(e => e.ServiceTitle).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<AdoptionRequestsHome>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Adoption__33A8517AB82691CF");

            entity.ToTable("AdoptionRequestsHome");

            entity.Property(e => e.RequestedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RequesterAddress).HasMaxLength(255);
            entity.Property(e => e.RequesterEmail).HasMaxLength(150);
            entity.Property(e => e.RequesterName).HasMaxLength(100);
            entity.Property(e => e.RequesterPhone).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("pending");

            entity.HasOne(d => d.Pet).WithMany(p => p.AdoptionRequestsHomes)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("FK__AdoptionR__PetId__66B53B20");
        });

        modelBuilder.Entity<AdoptionRequestsStray>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Adoption__33A8517AD2846BCF");

            entity.ToTable("AdoptionRequestsStray");

            entity.Property(e => e.RequestedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RequesterAddress).HasMaxLength(255);
            entity.Property(e => e.RequesterEmail).HasMaxLength(150);
            entity.Property(e => e.RequesterName).HasMaxLength(100);
            entity.Property(e => e.RequesterPhone).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("pending");

            entity.HasOne(d => d.Pet).WithMany(p => p.AdoptionRequestsStrays)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("FK__AdoptionR__PetId__6B79F03D");
        });

        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContactM__3214EC07AE90D613");

            entity.ToTable("ContactMessage");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Subject).HasMaxLength(150);
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Faq>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Faqs__3214EC07E3663689");

            entity.Property(e => e.Question).HasMaxLength(500);
        });

        modelBuilder.Entity<Ngo>(entity =>
        {
            entity.HasKey(e => e.NgoId).HasName("PK__Ngos__94E56EE3AAF9D548");

            entity.HasIndex(e => e.Email, "UQ__Ngos__A9D105347A53CF22").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.AvailabilityStatus).HasDefaultValue(true);
            entity.Property(e => e.Branches).HasMaxLength(300);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.NgoName).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.PetId).HasName("PK__Pets__48E538623AE723EF");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImagePath).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.PetName).HasMaxLength(100);
        });

        modelBuilder.Entity<PetCareGuideline>(entity =>
        {
            entity.HasKey(e => e.GuidelineId).HasName("PK__PetCareG__65D06B68537559F2");

            entity.Property(e => e.Behavior).HasMaxLength(500);
            entity.Property(e => e.Food).HasMaxLength(255);
            entity.Property(e => e.Precautions).HasMaxLength(500);

            entity.HasOne(d => d.Pet).WithMany(p => p.PetCareGuidelines)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("FK__PetCareGu__PetId__07C12930");
        });

        modelBuilder.Entity<PetCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PetCateg__3214EC0744AA9855");

            entity.ToTable("PetCategory");

            entity.Property(e => e.ActionName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<PetsStray>(entity =>
        {
            entity.HasKey(e => e.PetId).HasName("PK__PetsStra__48E538624C00E85D");

            entity.ToTable("PetsStray");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImagePath).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3214EC07F5F393F8");

            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Position).HasMaxLength(100);
        });

        modelBuilder.Entity<UserRegistration>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__USER_REG__F3BEEBFF7DE9716D");

            entity.ToTable("USER_REGISTRATION");

            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_EMAIL");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_PASSWORD");
            entity.Property(e => e.UserRole)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // REVIEW: Using Set<T>() ensures these properties are never null 
    // and avoids compiler warnings in modern .NET.
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>(); 
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- PATIENT CONFIGURATION ---
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // ENHANCEMENT: Unique Index on PatientCode
            // This is our "Safety Net" to ensure no two patients ever have the same code.
            entity.Property(e => e.PatientCode).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.PatientCode).IsUnique();

            // ENHANCEMENT: NationalId Index
            // In Kenya 2026, the Digital Health Act requires unique ID tracking.
            // This index prevents duplicate registrations.
            entity.HasIndex(e => e.NationalId).IsUnique();

            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
        });

        // --- DOCTOR CONFIGURATION ---
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DoctorCode).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.DoctorCode).IsUnique();
        });

        // --- APPOINTMENT CONFIGURATION ---
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);

        // ENHANCEMENT: Indexing the Code for high-speed lookups in busy clinics

            entity.Property(e => e.AppointmentCode)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(e => e.AppointmentCode)
                .IsUnique();
        // ENHANCEMENT: Enum Conversion
        // We store the Enum as a string ("CheckedIn" vs 1) in the DB. 
         // This makes the database readable for external PowerBI or Excel reports.
            entity.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // REVIEW: Relationships
            // We use .Restrict to prevent accidental deletion of a patient 
            // who still has appointment history (Medical Audit Trail).
            entity.HasOne(e => e.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    // REVIEW: Implementation of the Interface
    // This allows your Application layer to call SaveChanges without knowing about the DB.
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}

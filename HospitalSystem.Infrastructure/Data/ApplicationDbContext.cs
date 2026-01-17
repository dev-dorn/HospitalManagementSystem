using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using HospitalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Infrastructure.Data;

public class ApplicationDbContext : ApplicationDbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        :base(options)
    {
        
    }
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Patient> Doctors => Set<Doctor>();
    public Dbset<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //patient configuration
        mnodelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PatientCode)
                .IsRequired()
                .HasMaxLength(20);
            entity.HasIndex(e => e.PatientCode)
                .IsUnique();
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength();
            entity.Property(entity => entity.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
            

            

        });
        //Doctor configuration
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e = entity.Id);
            entity.Property(e = entity.DoctorCode)
                .IsRequired()
                .HasMaxLength(20);
            entity.HasIndex(e => e.DoctorCode)
                .IsUnique();
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

        });
        // Appointment configuration
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AppointmentCode)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(e => e.AppointmentCode)
                .IsUnique();

            entity.HasOne(e => e.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehaviour.Restrict);

            entity.HasOne(e => e.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(e => DoctorId)
                .OnDelete(DeleteBehaviour.Restrict);

            
             
        });
    }


}
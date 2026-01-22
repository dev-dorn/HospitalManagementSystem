using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace HospitalSystem.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // --- CLINICAL TRIANGLE ---
    DbSet<Patient> Patients { get; }
    DbSet<Doctor> Doctors { get; }
    DbSet<Appointment> Appointments { get; }

    // --- CLINICAL RECORDS ---
    // FIX: Corrected typo 'MedicaRecord' to 'MedicalRecord'
    DbSet<MedicalRecord> MedicalRecords { get; }
    
    // FIX: Pluralized 'Prescription' to 'Prescriptions' for consistency
    DbSet<Prescription> Prescriptions { get; }

    // --- INVENTORY ---
    DbSet<Drug> Drugs { get; }

    // --- PERSISTENCE ---
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    // ENHANCEMENT: Transaction Support
    // We need this for the "Two-Step Save" (Save 1: Get ID, Save 2: Update Code).
    // This allows the Handler to use: using var transaction = await _context.Database.BeginTransactionAsync();
    DatabaseFacade Database { get; }
}

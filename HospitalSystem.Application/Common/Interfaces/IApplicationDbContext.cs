using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Patient> Patients {get;}
    DbSet<Doctor> Doctors {get;}
    DbSet<Appointment> Appointments {get;}

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
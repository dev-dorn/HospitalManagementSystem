using AutoMapper;
using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Application.Appointments.DTOs;
using HospitalSystem.Domain.Entities;
using HospitalSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Appointments.Commands;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, AppointmentDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateAppointmentCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AppointmentDto> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        // 1. Validation: Ensure the Doctor exists and is active
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == request.DoctorId, cancellationToken);

        if (doctor == null) 
            throw new KeyNotFoundException($"Doctor with ID {request.DoctorId} not found.");

        if (!doctor.IsAvailable)
            throw new InvalidOperationException("This doctor is currently not available for new bookings.");

        // 2. Validation: Ensure the Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Id == request.PatientId, cancellationToken);

        if (!patientExists)
            throw new KeyNotFoundException($"Patient with ID {request.PatientId} not found.");

        // 3. Start Transaction for the Two-Phase Save
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var appointment = _mapper.Map<Appointment>(request);
            
            // ENHANCEMENT: Inherit the fee from the Doctor's profile at the time of booking
            appointment.ConsultationFee = doctor.ConsultationFee;
            appointment.Status = AppointmentStatus.Scheduled;
            appointment.AppointmentCode = "PENDING";
            appointment.CreatedAt = DateTime.UtcNow;

            // 4. Save Phase 1: Generate the Identity ID
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);

            // 5. Generate unique code (e.g., APP-2026-00001)
            appointment.GenerateAppointmentCode();

            // 6. Save Phase 2: Update the record
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return _mapper.Map<AppointmentDto>(appointment);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

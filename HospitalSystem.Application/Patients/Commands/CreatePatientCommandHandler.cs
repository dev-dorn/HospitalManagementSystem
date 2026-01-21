using AutoMapper;
using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Application.Patients.DTOs;
using HospitalSystem.Domain.Entities;
using MediatR;


namespace HospitalSystem.Application.Patients.Commands;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, PatientDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreatePatientCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }
    public async Task<PatientDto> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
{
    if (request == null) throw new ArgumentNullException(nameof(request));

    // Start a transaction to ensure both saves are treated as one unit of work
    using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

    try 
    {
        var patient = _mapper.Map<Patient>(request);
        patient.CreatedAt = DateTime.UtcNow;
        patient.PatientCode = Patient.DefaultPatientCode;

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync(cancellationToken); // Save 1 (Gets ID)

        patient.GeneratePatientCode();
        await _context.SaveChangesAsync(cancellationToken); // Save 2 (Updates Code)

        await transaction.CommitAsync(cancellationToken);
        
        return _mapper.Map<PatientDto>(patient);
    }
    catch (Exception)
    {
        await transaction.RollbackAsync(cancellationToken);
        throw; // Re-throw to be handled by your Global Exception Filter
    }
}
}
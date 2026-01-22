using AutoMapper;
using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using MediatR;
using HospitalSystem.Application.Doctors.DTOs;
namespace HospitalSystem.Application.Doctors.Commands;

public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, DoctorDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateDoctorCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DoctorDto> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var doctor = _mapper.Map<Doctor>(request);
            doctor.DoctorCode = "PENDING";

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync(cancellationToken);

            // Domain Logic: Generate DOC-XXX based on DB ID
            doctor.GenerateDoctorCode();
            
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return _mapper.Map<DoctorDto>(doctor);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

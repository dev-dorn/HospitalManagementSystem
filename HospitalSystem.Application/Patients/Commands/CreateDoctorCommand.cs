using MediatR;
using HospitalSystem.Application.Doctors.DTOs;

namespace HospitalSystem.Application.Doctors.Commands;

public class CreateDoctorCommand : IRequest<DoctorDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    // ENHANCEMENT: Required for Kenya 2026 Legal Compliance
    public string LicenseNumber { get; set; } = string.Empty; 
    
    public string Specialization { get; set; } = string.Empty;
    public string? Qualification { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public decimal ConsultationFee { get; set; }
}

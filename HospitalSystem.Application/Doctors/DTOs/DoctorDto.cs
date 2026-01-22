namespace HospitalSystem.Application.Doctors.DTOs;

public class DoctorDto
{
    public int Id { get; set; }
    public string DoctorCode { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public bool IsAvailable { get; set; }
    
    public string FullName => $"Dr. {FirstName} {LastName}";
}

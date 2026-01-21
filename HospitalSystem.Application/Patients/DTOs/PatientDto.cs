namespace HospitalSystem.Application.Patients.DTOs;

public class PatientDto
{
    public int Id { get; set; }
    public string PatientCode { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    
    // ENHANCEMENT: FullName helper for the UI (to avoid logic in the frontend)
    public string FullName => string.IsNullOrWhiteSpace(MiddleName) 
        ? $"{FirstName} {LastName}" 
        : $"{FirstName} {MiddleName} {LastName}";

    public DateTime DateOfBirth { get; set; }

    // REVIEW: We use a string here for the DTO (mapped from the Enum) 
    // to make it easy for JSON consumers.
    public string Gender { get; set; } = string.Empty;
    public string? BloodGroup { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }

    // ENHANCEMENT: Kenya-specific identifiers for Insurance/SHA lookups
    public string NationalId { get; set; } = string.Empty;
    public string? SHANumber { get; set; }

    // REVIEW: Age calculation is excellent.
    public int Age => CalculateAge();

    private int CalculateAge()
    {
        // ENHANCEMENT: In 2026, we use DateTime.UtcNow to ensure consistency 
        // across different server/client time zones.
        var today = DateTime.UtcNow.Date;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}

namespace HospitalSystem.Application.Patients.DTOs;

public class CreatePatientDto
{
    // --- BASIC INFO ---
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }

    // ENHANCEMENT: Required for Kenya 2026 Digital Health Act
    // This must be captured during registration.
    public string NationalId { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty; // Maps to our Gender Enum
    public string? BloodGroup { get; set; }

    // --- CONTACTS ---
    public string PhoneNumber { get; set; } = string.Empty;
    public string? AlternatePhone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }

    // --- EMERGENCY & INSURANCE ---
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    
    // ENHANCEMENT: Renaming to match current 2026 Kenyan Health Authority (SHA)
    public string? SHANumber { get; set; } 
    public string? InsuranceProvider { get; set; }

    // --- COMPLIANCE ---
    // ENHANCEMENT: Proof of consent is legally required before digitizing records.
    public bool DataProcessingConsent { get; set; } = false;
}

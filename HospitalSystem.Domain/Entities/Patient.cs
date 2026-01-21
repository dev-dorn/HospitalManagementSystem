using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class Patient : BaseEntity
{
    // --- IDENTIFICATION ---
    public string PatientCode { get; set; } = DefaultPatientCode;
    
    // ENHANCEMENT: Mandatory for Kenya 2026 Digital Health Act
    // Used for IPRS verification and preventing duplicate files.
    public string NationalId { get; set; } = string.Empty; 

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty; 
    public string? BloodGroup { get; set; }

    // --- CONTACT DETAILS ---
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
    
    // ENHANCEMENT: Renamed for 2026 clarity (The new NHIF is SHA)
    public string? SHANumber { get; set; } 
    public string? InsuranceProvider { get; set; }

    // --- COMPLIANCE (Kenya Data Protection Act) ---
    // REVIEW: Clinics MUST track consent to digitize records in 2026.
    public bool HasConsentedToDataProcessing { get; set; }
    public DateTime? ConsentDate { get; set; }

    // --- NAVIGATION PROPERTIES ---
    // REVIEW: Initializing as HashSet prevents NullReferenceExceptions 
    // and is more performant for large sets of data.
    public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new HashSet<MedicalRecord>();

    // --- DOMAIN LOGIC ---
    public const string DefaultPatientCode = "PENDING";

    /// <summary>
    /// Generates a unique patient code. 
    /// REVIEW: Must be called after SaveChanges() when Id is populated.
    /// </summary>
    public void GeneratePatientCode()
    {
        if (Id <= 0) 
            throw new InvalidOperationException("Cannot generate code without a valid Database ID.");

        // ENHANCEMENT: D6 allows for 1 million records (HOSP-000001).
        PatientCode = $"HOSP-{Id:D6}";
    }

    /// <summary>
    /// ENHANCEMENT: Helper to get the full name for UI displays.
    /// </summary>
    public string GetFullName() 
        => string.IsNullOrWhiteSpace(MiddleName) 
            ? $"{FirstName} {LastName}" 
            : $"{FirstName} {MiddleName} {LastName}";
}

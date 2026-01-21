using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class Doctor : BaseEntity
{
    // ENHANCEMENT: Unique identifier for internal use
    public string DoctorCode { get; set; } = "PENDING";
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    // ENHANCEMENT: KMPDC License Number (Legal Requirement in Kenya)
    // Every doctor in Kenya must have a valid license number (e.g., A1234).
    public string LicenseNumber { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty; // e.g., Pediatrician, GP
    public string? Qualification { get; set; } // e.g., MBChB, MMed
    
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    // REVIEW: Using a non-nullable decimal for the fee. 
    // Even if it's 0.00 (free), we should have a value for the accountant.
    public decimal ConsultationFee { get; set; } = 0.00m;
    
    public bool IsAvailable { get; set; } = true;

    // ENHANCEMENT: Navigation property naming fix (Pluralized)
    public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();

    // --- DOMAIN LOGIC ---

    public void GenerateDoctorCode()
    {
        if (Id <= 0) throw new InvalidOperationException("ID not set.");
        // Format: DOC-001
        DoctorCode = $"DOC-{Id:D3}";
    }

    public string GetFullName() => $"Dr. {FirstName} {LastName}";
}

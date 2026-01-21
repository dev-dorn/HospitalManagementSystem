using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Enums; // Required for the Enum

namespace HospitalSystem.Domain.Entities;

public class Appointment : BaseEntity
{
    // ENHANCEMENT: Default to 'PENDING' for the two-step save pattern we used for Patients
    public string AppointmentCode { get; set; } = "PENDING";
    
    public int PatientId { get; set; }
    public virtual Patient Patient { get; set; } = null!;

    public int DoctorId { get; set; }
    public virtual Doctor Doctor { get; set; } = null!;

    // REVIEW: Combining Date and Time into a single DateTime is usually 
    // better for EF Core queries and time-zone handling.
    public DateTime ScheduledStart { get; set; }
    
    // ENHANCEMENT: Track duration to prevent overbooking clinics
    public int DurationMinutes { get; set; } = 30;

    // FIX: Using our strongly-typed Enum instead of a string
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    public string? Reason { get; set; } // Chief complaint
    public string? Notes { get; set; }

    // --- FINANCIALS (Kenya 2026) ---
    // ENHANCEMENT: Tracking the fee and payment status
    public decimal ConsultationFee { get; set; } 
    public bool IsPaid { get; set; } = false;
    public string? PaymentReference { get; set; } // e.g., M-Pesa Transaction Code

    // --- DOMAIN LOGIC ---
    
    public void GenerateAppointmentCode()
    {
        if (Id <= 0) throw new InvalidOperationException("ID must be set.");
        // Format: APP-2026-00001
        AppointmentCode = $"APP-{DateTime.UtcNow.Year}-{Id:D5}";
    }

    public void CheckIn()
    {
        if (Status != AppointmentStatus.Scheduled)
            throw new InvalidOperationException("Only scheduled appointments can be checked in.");
            
        Status = AppointmentStatus.CheckedIn;
        // This is where you would trigger a Domain Event to notify the Doctor
    }
}

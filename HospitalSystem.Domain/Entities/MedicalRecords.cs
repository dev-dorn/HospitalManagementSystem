using HospitalSystem.Domain.Common;
namespace HospitalSystem.Domain.Entities;
public class MedicalRecord : BaseEntity
{
    // ---linkage ---
    public int PatientId {get; set;}
    public virtual Patient Patient {get; set;} = null!;
    public int DoctorId {get; set;}
    public virtual Doctor Doctor {get; set;} = null!;
    public int? DoctorId {get; set;} = null!;
    public int? AppointmentId {get; set;} //cant ve
    public virtual Appointment? Appointment {get; set;}
    //critical Data Soap format --
    //Subjective of what the patient shouhld says (symptoms chief complaints)
    public string ChiefComplaint {get; set; } = string.Empty;
    public string? VitalSignsSummary {get; set;} //eg Blood pressure
     //Assessment Diagnosis
     //EnhanceMent : 
    public string Diagnosis {get; set;} = string.Empty;
    public string? ICD11Code {get; set;}
     //Plan: Medications, Lab tests or Follow-up instructions
    public string TreatmentPlan {get; set;} = string.Empty;
    public string? PrescriptionNotes {get; set;}
    //Attachment & Metadata ---
    //lab results
    public string? LabResultSummary {get; set;}
    //enhancement: Digital signature (legal reuirement for electronic health records)
    //this tracks if the doctor has "locked" the record.
    public bool IsFinalized {get; set;} = false;
    public DateTime? FinalizedAt {get; set;}

    //---Domain Logic --
    public void FinalizedRecord()
    {
        if (string.IsNullOrWhiteSpace(Diagnosis))
        {
            throw new InvalidOperationException("Cannot finalize a record without diagnosis.");

        }
        IsFinalized = true;
        FinalizedAt = Datetime.UtcNow;
    }
}
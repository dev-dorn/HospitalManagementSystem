namespace HospitalSystem.Domain.Entities;

public class Appointment: BaseEntity
{
    public string AppointmentCode {get; set; } = string.Empty;
    public int PatientId {get; set;}
    public int DoctorId {get; set;}
    public DateTime AppointmentDate {get; set;}
    public TimeSpan AppointmentTime {get; set;}
    public string Status {get; set;} = "Scheduled";
    public string? Reason {get; set;}
    public string? Notes {get; set;}
    public decimal? ConsultationFee {get; set;}

    public virtual Patient? Patient {get; set;}
    public virtual Doctor? Doctor {get; set;}
    

}
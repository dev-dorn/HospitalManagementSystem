namespace HospitalSystem.Domain.Entities;

public class Doctor : BaseEntity
{
    public string DoctorCode {get; set;} = string.Empty;
    public string FirstName{get; set;} = string.Empty;
    public string LastName{get; set;} = string.Empty;
    public string Specialization {get; set;}= string.Empty;
    public string Qualification {get; set;}
    public string PhoneNumber {get; set; } = string.Empty;
    public string Email {get; set;} = string.Empty;
    public decimal? ConsultationFee {get; set; }
    public bool IsAvailable {get; set; } = true;

    public virtual ICollection<Appointment>? Appointment {get; set;}

}
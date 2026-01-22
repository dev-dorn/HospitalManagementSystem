using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Application.Appointments.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public string AppointmentCode { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime ScheduledStart { get; set; }
    public AppointmentStatus Status { get; set; }
    public decimal ConsultationFee { get; set; }
}

using MediatR;
using HospitalSystem.Application.Appointments.DTOs;

namespace HospitalSystem.Application.Appointments.Commands;

public class CreateAppointmentCommand : IRequest<AppointmentDto>
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
}

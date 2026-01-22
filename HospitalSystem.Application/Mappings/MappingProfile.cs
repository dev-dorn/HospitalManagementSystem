using AutoMapper; // FIXED: Added this
using HospitalSystem.Application.Patients.Commands;
using HospitalSystem.Application.Patients.DTOs;
using HospitalSystem.Application.Doctors.Commands;
using HospitalSystem.Application.Doctors.DTOs;
using HospitalSystem.Application.Appointments.Commands;
using HospitalSystem.Application.Appointments.DTOs;
using HospitalSystem.Domain.Entities;

namespace HospitalSystem.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Patient Mappings
        CreateMap<CreatePatientCommand, Patient>();
        CreateMap<Patient, PatientDto>();

        // Doctor Mappings
        CreateMap<CreateDoctorCommand, Doctor>();
        CreateMap<Doctor, DoctorDto>();

        // Appointment Mappings
        CreateMap<CreateAppointmentCommand, Appointment>();
        CreateMap<Appointment, AppointmentDto>();
    }
}

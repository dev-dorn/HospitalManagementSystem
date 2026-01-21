using MediatR;
using HospitalSystem.Application.Patients.DTOs;

namespace HospitalSystem.Application.Patients.Commands;

public class CreatePatientCommand : IRequest<PatientDto>
{
    // --- BASIC INFO ---
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    
    // ENHANCEMENT: Required for 2026 Kenyan Digital Health Act
    public string NationalId { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
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
    
    // ENHANCEMENT: Alignment with SHA (Social Health Authority)
    public string? SHANumber { get; set; } 
    public string? InsuranceProvider { get; set; }

    // --- COMPLIANCE ---
    // ENHANCEMENT: Legal requirement for ODPC 2026
    public bool HasConsentedToDataProcessing { get; set; }
}

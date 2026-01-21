using HospitalSystem.Application.Patients.DTOs;
using MediatR;

namespace HospitalSystem.Application.Patients.Commands;

public class CreatePatientCommand : IRequest<PatientDto>
{
    public string FirstName {get; set;} = string.Empty;

    public string LastName {get; set;} = string.Empty;
    public string? MiddleName {get; set;}
    public DateTime DateOfBirth {get; set;}
    public string Gender {get; set;} = string.Empty;
    public string? BloodGroup {get; set;}
    public string PhoneNumber {get; set;} = string.Empty;
    public string? AlternatePhone {get; set;}
    public string? Email {get; set;}
    public string? Address {get; set;}
    public string? City {get; set;}
    public string? ZipCode {get; set;}
    public string? EmergencyContactName {get; set;}
    public string? EmergencyContactPhone {get; set;}
    public string? InsuranceId {get; set;}
    public string? InsuranceProvider {get; set;}

}
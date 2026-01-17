namespace HospitalSystem.Application.Patients.DTOs;

public class PatientDto
{
    public int Id {get; set;}
    public string PatientCode {get; set; } = string.Empty;
    public string FirstName {get; set; } = string.Empty;
    public string LastName {get; set; } = string.Empty;
    public string? MiddleName {get; set;}
    public DateTime DateOfBirth {get; set;}

    public string Gender {get; set;} = string.Empty;
    public string? BloodGroup {get; set;}
    public string PhoneNumber {get; set;} = string.Empty;
    public string? Email {get; set;}
    public string? Address {get; set ;}
    public string? EmergencyContactName {get; set;}
    public string? EmergencyContactPhone {get; set;}
    public int Age => CalculateAge();
    private int CalculateAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

}
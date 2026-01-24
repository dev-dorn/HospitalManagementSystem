namespace HospitalSystem.Application.Patients.DTOs;

public class CreatePatientDto
{
    // --- BASIC INFO ---
    [Required(ErrrMessage ="Frst name is required")]
    [StringLength(100, MinimunLength = 2, ErrorMessage= "First name must be between 2 and 100  characters")]
    [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "First name only contain letters, spaces, hyphens, and apostroophes ")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErroMessage = "Last name is required")]
    [StringLength(100, MinimumLength=2, ErrorMessage = "last name must between 2 and 100 characters")]
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }

    // ENHANCEMENT: Required for Kenya 2026 Digital Health Act
    // This must be captured during registration.
    [Required(ErrorMessage ="National ID or Birth Certificates No. is required")]
    [RequiredExpression(@"^\d{7,10}$", ErrorMessage = "Invalid Identification Number format")]

    public string NationalId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    public string Gender { get; set; } = string.Empty; 
    // Maps to our Gender Enum
    [StringLength(5)]
    public string? BloodGroup { get; set; }


    // --- CONTACTS ---
    [Required(ErrorMessage = "Please enter a valid phonenumber")]
    [RegularExpression(@"^(?:254|\+254|0)?(7|1)\d{8}$", ErrorMessage = "Inavlid Phone number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a valid phonenumber")]
    [RegularExpression(@"^(?:254|\+254|0)?(7|1)\d{8}$", ErrorMessage = "Inavlid Phone number")]
    public string? AlternatePhone { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(200, ErroMessage = "Email cannot exceed 200 characters")]
    [CustomValidation(typeof(PatientValidation), nameof(ValidateEmailUniqueness))]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Address is required")]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 500 characters")]
    public string? Address { get; set; }
    [StringLength(100, ErrorMessage ="state cannot exceed 50 characters")]
    public string? City { get; set; }
    [StringLength(50, ErrorMessage = "State cannot exceed 50 charcaters")]

    public string? State { get; set; }
    [StringLength(20, ErrorMessage = "Zip code cannot exceed 20 characters")]
    [RegularExpression(@"^[0-9\-]+$", ErrorMessage = "Zip code can only contain numbers and hyphens")]
    public string? ZipCode { get; set; }

    // --- EMERGENCY CONTACT ---
    [StringLength(100, ErrorMessage = "Emergency contact name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "Name can only contain letters and spaces")]
    public string? EmergencyContactName { get; set; }

    [StringLength(20)]
    [RegularExpression(@"^(?:254|\+254|0)?(7|1)\d{8}$", ErrorMessage = "Invalid Kenyan phone number format for emergency contact")]
    public string? EmergencyContactPhone { get; set; }
    
    // --- INSURANCE & SHA (SOCIAL HEALTH AUTHORITY) ---
    // ENHANCEMENT: SHA Number is now the primary key for government-sponsored healthcare in Kenya.
    [Required(ErrorMessage = "SHA Number is required for claim processing")]
    [RegularExpression(@"^[A-Z0-9]{5,20}$", ErrorMessage = "Invalid SHA Number format")]
    public string? SHANumber { get; set; } 

    [StringLength(100, ErrorMessage = "Insurance provider name cannot exceed 100 characters")]
    public string? InsuranceProvider { get; set; }

    // --- COMPLIANCE ---
    // ENHANCEMENT: Proof of consent is legally required before digitizing records.
    [Range(typeof(bool), "true", "true", ErrorMessage ="Patient must consent to data processing to be registered")]
    public bool DataProcessingConsent { get; set; } = false;
}

using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class Prescription : BaseEntity
{
    public int MedicalRecordId {get; set;}
    public virtual MedicalRecord MedicalRecord {get; set;} = null!;
    public int DrugId {get; set;}
    public virtual Drug Drug {get; set;} = null!;
    public string Dosage {get; set;} = string.Empty; //e.g, "500mg"
    public string Frequency {get; set;} = string.Empty; //e.g , "BD" (Twice Daily)
    public int Duration {get; set;}// e.g ., 7 (days)

    //Enhancement : quantity calculation
    //Important for the pharmacy to know the exactly how many tablets/bottles to give
    public int TotalQuantity {get; set;}
    public string? Instructions {get; set;} //e.g "Take after meals

    // --- DISPENCING STATUS ---
    public bool IsDispensed {get; set;} = false;
    public DateTime? DispensedAt {get; set;}

    //ENHANCEMENT: Audit Trail for pharmacy // in kenya this is essential
    public string? DispensedBy {get; set;}

    // ---Safety & Compliance ---
    //Enhancement: refill logic
    //Common for chronic conditions (Hypertension, Diabetes)
    public int AllowedRefills {get; set;} = 0;
    public int RefillsUsed {get; set;} = 0;

    //Enhancement: Electronic Signature (Legal Requirement)
    public string PrescriberLicenseNumber{get; set;} = string.Empty;


}
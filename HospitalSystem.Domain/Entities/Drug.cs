using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class Drug : BaseEntity
{
    // ENHANCEMENT: Unique Drug Identifier
    public string DrugCode { get; set; } = string.Empty; // e.g., "D-0001"
    
    [Required]
    public string Name { get; set; } = string.Empty; // Brand Name (e.g., Panadol)
    
    public string? GenericName { get; set; } // Chemical Name (e.g., Paracetamol)
    public string? Manufacturer { get; set; }
    public string? Category { get; set; } // e.g., Antibiotics, Analgesics
    
    // ENHANCEMENT: Packaging vs Unit
    public string? DrugType { get; set; } // e.g., Tablet, Syrup, Injection
    public string? Strength { get; set; } // e.g., 500mg, 10ml
    
    // --- FINANCIALS ---
    public decimal UnitPrice { get; set; } // Selling Price
    public decimal CostPrice { get; set; } // ENHANCEMENT: To calculate Clinic Profitability

    // --- INVENTORY MANAGEMENT ---
    public int CurrentStock { get; set; }
    public int MinimumStockLevel { get; set; } = 10;
    
    // ENHANCEMENT: Tracking Batches (Critical for 2026 Safety)
    // A single drug type might have multiple batches with different expiry dates.
    public string? BatchNumber { get; set; } 
    public DateTime? ExpiryDate { get; set; }

    // ENHANCEMENT: PPB Classification
    // Prescription Only (POM), Pharmacy Only (P), General Sale (GSL)
    public string? PrescriptionClass { get; set; }

    // --- NAVIGATION ---
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new HashSet<Prescription>();
    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new HashSet<InventoryTransaction>();

    // --- DOMAIN LOGIC ---
    public bool IsLowStock => CurrentStock <= MinimumStockLevel;
    
    public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value.Date <= DateTime.UtcNow.Date;

    // A helper to warn the pharmacist if the drug is near expiry (within 90 days)
    public bool IsNearExpiry => ExpiryDate.HasValue && 
                               ExpiryDate.Value.Date <= DateTime.UtcNow.AddMonths(3).Date;
}

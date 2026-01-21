using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSystem.Domain.Common;

public abstract class BaseEntity
{
    // FIX: Moved PatientCode to the Patient.cs file. 
    // BaseEntity should only contain fields that EVERY table uses.
    
    public int Id { get; set; }
    
    // ENHANCEMENT: Audit trails are vital for Medical Audits in 2026.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; } // Tracks which nurse/admin created the record
    
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; } // Tracks who made the last change
    
    public bool IsActive { get; set; } = true;
    
    // ENHANCEMENT: Soft Delete support. 
    // In Healthcare, you NEVER permanently delete medical data (legal requirement).
    public bool IsDeleted { get; set; } = false;

    #region Domain Events
    // REVIEW: Domain Events allow parts of your system to react when something happens.
    // E.g., When a Patient is created, an "SmsNotificationEvent" can be triggered.

    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped] // Ensures this list isn't turned into a database column
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    #endregion
}

public abstract class BaseEvent
{
    // REVIEW: In 2026, using DateTimeOffset is better for Kenya-wide systems 
    // to handle time zones accurately across different counties.
    public DateTimeOffset OccurredOn { get; protected set; } = DateTimeOffset.UtcNow;
}

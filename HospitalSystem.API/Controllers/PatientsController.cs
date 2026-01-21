using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Application.Patients.DTOs;
using HospitalSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<PatientsController> _logger;
    
    public PatientsController(IApplicationDbContext context, ILogger<PatientsController> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
    {
        try
        {
            var patients = await _context.Patients
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
                
            var patientDtos = patients.Select(p => new PatientDto
            {
                Id = p.Id,
                PatientCode = p.PatientCode,
                FirstName = p.FirstName,
                LastName = p.LastName,
                MiddleName = p.MiddleName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                BloodGroup = p.BloodGroup,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Address = p.Address,
                EmergencyContactName = p.EmergencyContactName,
                EmergencyContactPhone = p.EmergencyContactPhone
            });
            
            return Ok(patientDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting patients");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> GetPatient(int id)
    {
        try
        {
            var patient = await _context.Patients.FindAsync(id);
            
            if (patient == null)
            {
                return NotFound();
            }
            
            var patientDto = new PatientDto
            {
                Id = patient.Id,
                PatientCode = patient.PatientCode,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                BloodGroup = patient.BloodGroup,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                Address = patient.Address,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyContactPhone = patient.EmergencyContactPhone
            };
            
            return Ok(patientDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting patient with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<PatientDto>> CreatePatient(Patient patient)
    {
        try
        {
            // Generate patient code
            var lastPatient = await _context.Patients
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();
                
            var nextId = (lastPatient?.Id ?? 0) + 1;
            patient.PatientCode = $"HOSP-{nextId:0000}";
            patient.CreatedAt = DateTime.UtcNow;
            
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            
            var patientDto = new PatientDto
            {
                Id = patient.Id,
                PatientCode = patient.PatientCode,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                BloodGroup = patient.BloodGroup,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                Address = patient.Address,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyContactPhone = patient.EmergencyContactPhone
            };
            
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patientDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(int id, Patient updatedPatient)
    {
        try
        {
            var patient = await _context.Patients.FindAsync(id);
            
            if (patient == null)
            {
                return NotFound();
            }
            
            // Update properties
            patient.FirstName = updatedPatient.FirstName;
            patient.LastName = updatedPatient.LastName;
            patient.MiddleName = updatedPatient.MiddleName;
            patient.DateOfBirth = updatedPatient.DateOfBirth;
            patient.Gender = updatedPatient.Gender;
            patient.BloodGroup = updatedPatient.BloodGroup;
            patient.PhoneNumber = updatedPatient.PhoneNumber;
            patient.Email = updatedPatient.Email;
            patient.Address = updatedPatient.Address;
            patient.EmergencyContactName = updatedPatient.EmergencyContactName;
            patient.EmergencyContactPhone = updatedPatient.EmergencyContactPhone;
            patient.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        try
        {
            var patient = await _context.Patients.FindAsync(id);
            
            if (patient == null)
                return NotFound();
            
            
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
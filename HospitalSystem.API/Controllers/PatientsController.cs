using AutoMapper;
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
    private readonly IMapper _mapper;

    public PatientsController(IApplicationDbContext context, ILogger<PatientsController> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    // GET: api/Patients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients(CancellationToken ct)
    {
        try
        {
            var patients = await _context.Patients
                .Where(p => !p.IsDeleted) // Soft-delete filter
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(ct);

            return Ok(_mapper.Map<IEnumerable<PatientDto>>(patients));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching patients list");
            return StatusCode(500, "Internal server error");
        }
    }

    // GET: api/Patients/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> GetPatient(int id, CancellationToken ct)
    {
        try
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, ct);

            if (patient == null) return NotFound();

            return Ok(_mapper.Map<PatientDto>(patient));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching patient {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: api/Patients
    [HttpPost]
    public async Task<ActionResult<PatientDto>> CreatePatient(CreatePatientDto dto, CancellationToken ct)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(ct);
        try
        {
            var patient = _mapper.Map<Patient>(dto);
            patient.CreatedAt = DateTime.UtcNow;
            patient.PatientCode = "PENDING";

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync(ct);

            // Generate unique code based on the new DB ID
            patient.GeneratePatientCode();
            await _context.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

            var resultDto = _mapper.Map<PatientDto>(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, resultDto);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            _logger.LogError(ex, "Error creating patient");
            return BadRequest("Could not create patient record");
        }
    }

    // PUT: api/Patients/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(int id, CreatePatientDto dto, CancellationToken ct)
    {
        try
        {
            var patient = await _context.Patients.FindAsync(new object[] { id }, ct);

            if (patient == null || patient.IsDeleted) return NotFound();

            _mapper.Map(dto, patient); // Update existing entity with DTO values
            patient.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    // DELETE: api/Patients/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id, CancellationToken ct)
    {
        try
        {
            var patient = await _context.Patients.FindAsync(new object[] { id }, ct);

            if (patient == null) return NotFound();

            // 2026 Medical Standard: Soft Delete
            patient.IsDeleted = true;
            patient.IsActive = false;

            await _context.SaveChangesAsync(ct);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}

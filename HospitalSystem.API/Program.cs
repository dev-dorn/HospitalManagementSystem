using HospitalSystem.Infrastructure.Data;
using HospitalSystem.Domain.Entities; // Required for 'Patient' and 'Doctor'
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using HospitalSystem.Infrastructure; // Ensures .AddInfrastructure() is found

var builder = WebApplication.CreateBuilder(args);

// --- SERVICES ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hospital Management System API",
        Version = "v1",
        Description = "API for managing hospital operations in Kenya"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Load DB, MediatR, and AutoMapper via your Infrastructure/Application extensions
builder.Services.AddInfrastructure(builder.Configuration);
// builder.Services.AddApplication(); // Ensure this is called to register MediatR!

var app = builder.Build();

// --- MIDDLEWARE ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// FIX: 'UseHttpRedirection' was misspelled as 'UseHttpRedirection' (double-check casing)
app.UseHttpsRedirection(); 
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// --- SEED DATA ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        
        // Use Migrate() instead of EnsureCreated() if using Migrations
        dbContext.Database.EnsureCreated();

        if (!dbContext.Patients.Any())
        {
            dbContext.Patients.AddRange(
                new Patient
                {
                    PatientCode = "HOSP-000001",
                    FirstName = "John",
                    LastName = "Doe",
                    NationalId = "12345678", // Added for 2026 Compliance
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = "Male",
                    PhoneNumber = "0712345678",
                    Email = "john.doe@example.com",
                    HasConsentedToDataProcessing = true,
                    CreatedAt = DateTime.UtcNow // FIX: was DateTime.Utc.Now
                }
            );

            dbContext.Doctors.AddRange(
                new Doctor
                {
                    DoctorCode = "DOC-001",
                    FirstName = "Robert",
                    LastName = "Wilson",
                    LicenseNumber = "KMPDC-A123", // Added for Kenya 2026 Compliance
                    Specialization = "Cardiology",
                    Qualification = "MD, Cardiology",
                    PhoneNumber = "0722000111",
                    ConsultationFee = 3000.00m, // KES
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

            dbContext.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // For testing, write to console if seeding fails
        Console.WriteLine($"An error occurred seeding the DB: {ex.Message}");
    }
}

app.Run();

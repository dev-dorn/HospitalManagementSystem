using HospitalSystem.Infrastructure.Data;
using HospitalSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --- SERVICES ---
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
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

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// --- MIDDLEWARE ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
        dbContext.Database.EnsureCreated();

        if (!dbContext.Patients.Any())
        {
            dbContext.Patients.Add(new Patient
            {
                PatientCode = "HOSP-000001",
                FirstName = "John",
                LastName = "Doe",
                NationalId = "12345678",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                PhoneNumber = "0712345678",
                Email = "john.doe@example.com",
                HasConsentedToDataProcessing = true,
                CreatedAt = DateTime.UtcNow
            });

            dbContext.Doctors.Add(new Doctor
            {
                DoctorCode = "DOC-001",
                FirstName = "Robert",
                LastName = "Wilson",
                LicenseNumber = "KMPDC-A123",
                Specialization = "Cardiology",
                Qualification = "MD, Cardiology",
                PhoneNumber = "0722000111",
                ConsultationFee = 3000.00m,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            });

            dbContext.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred seeding the DB: {ex.Message}");
    }
}

app.Run();

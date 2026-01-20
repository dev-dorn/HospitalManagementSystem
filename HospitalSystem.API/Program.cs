using HospitalSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//add services to the container

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",  new OpenApiInfo
    {
        Title = "Hospital Management System API",
        Version = "v1",
        Description = "API for managing hospital operations"
    });
});
// add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//Add Infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

//Http pipeline for requests
if (app.Enviroment.IsDeveopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Seed some initial data

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    // Seed test data is the database is empty
    if (dbContext.Patients.Any())
    {
        dbContext.Patients.AddRange(
            new Patient
            {
                PatientCode = "HOSP-001",
                FirstName = "John",
                LastName = "DOE",
                DateofBirth= new DateTime(1990, 1,1),
                Gender = "Male",
                PhoneNumber = "12345678",
                Email = "john.doe@example.com",
                Address = "12  Main str",
                City = "Nairobi",
                State = "Kenya",
                CreatedAt = DateTime.Utc.Now         
                
            },
            new Patient
            {
                PatientCode = "HOSP-002",
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 15),
                Gender = "Female",
                PhoneNumber = "0987654321",
                Email = "jane.smith@example.com",
                Address = "456 Oak Ave",
                City = "Los Angeles",
                State = "CA",
                CreatedAt = DateTime.UtcNow
            }
        );

        dbContext.Doctors.AddRange(
            new Doctor
            {
                DoctorCode = "DOC-001",
                FirstName = "Robert",
                LastName = "Wilson",
                Specialization = "Cardiology",
                Qualification = "MD, Cardiology",
                PhoneNumber = "555-0101",
                Email = "r.wilson@hospital.com",
                ConsultationFee = 50.00m,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            },
            new Doctor
            {
                DoctorCode = "DOC-002",
                FirstName = "Sarah",
                LastName = "Miller",
                Specialization = "Pediatrics",
                Qualification = "MD, Pediatrics",
                PhoneNumber = "555-0102",
                Email = "s.miller@hospital.com",
                ConsultationFee = 45.00m,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            }
        );
        
        dbContext.SaveChanges();
    }


}
app.Run();
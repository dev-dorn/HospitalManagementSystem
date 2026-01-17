using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UserInMemoryDatabase("HospitalDb"));

        service.AddScoped<IApplicationDbContext>(provider =>
        
            provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
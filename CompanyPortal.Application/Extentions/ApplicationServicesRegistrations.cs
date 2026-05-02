using CompanyPortal.Application.Mapper;
using CompanyPortal.Application.Services.Implementations;
using CompanyPortal.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyPortal.Application.Extentions;

public static class ApplicationServicesRegistrations
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(EmployeeMappingProfile));

        services.AddScoped<IEmployeeService, EmployeeService>();

        return services;
    }
}

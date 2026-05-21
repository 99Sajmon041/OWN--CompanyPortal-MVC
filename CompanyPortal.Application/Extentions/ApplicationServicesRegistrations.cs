using CompanyPortal.Application.Mapper;
using CompanyPortal.Application.Services.Implementations;
using CompanyPortal.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyPortal.Application.Extentions;

public static class ApplicationServicesRegistrations
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IBuildingService, BuildingService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IEmployeeDeviceService, EmployeeDeviceService>();

        return services;
    }
}

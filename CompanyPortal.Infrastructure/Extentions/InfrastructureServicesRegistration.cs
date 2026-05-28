using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.AppUser;
using CompanyPortal.Infrastructure.Database;
using CompanyPortal.Infrastructure.Repositories;
using CompanyPortal.Web.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyPortal.Infrastructure.Extentions;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CompanyPortalDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<CompanyPortalDbContext>();

        services.Configure<SeedAdmin>(
            configuration.GetSection("SeedAdmin"));

        services.Configure<List<string>>(
            configuration.GetSection("SeedRoles"));

        services.Configure<SeedAdminEmployee>(
            configuration.GetSection("SeedAdminEmployee"));

        services.Configure<SeedPosition>(
            configuration.GetSection("SeedPosition"));

        services.Configure<SeedBuilding>(
            configuration.GetSection("SeedBuilding"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IApplicationUserService, ApplicationUserService>();
        services.AddScoped<IRoleAssignmentService, RoleAssignmentService>();

        return services;
    }
}
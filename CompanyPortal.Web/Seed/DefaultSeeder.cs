using CompanyPortal.Domain.Entities;
using CompanyPortal.Infrastructure.AppUser;
using CompanyPortal.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CompanyPortal.Web.Seed;

public sealed class DefaultSeeder(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<SeedAdminOptions> userOptions,
    IOptions<List<string>> roleOptions,
    IOptions<SeedAdminEmployee> employeeOptions,
    IOptions<SeedPosition> positionsOptions,
    IOptions<SeedDepartment> departmentOptions,
    CompanyPortalDbContext context)
{
    public async Task SeedAdminUserAsync()
    {
        await SeedDefaultRolesAsync();

        if (await ExistsAdminUserAsync())
        {
            return;
        }

        await using var transaction = await context.Database.BeginTransactionAsync();

        var adminUser = new ApplicationUser
        {
            UserName = userOptions.Value.UserName,
            Email = userOptions.Value.Email,
            DomainUserName = userOptions.Value.DomainUserName,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            IsEmployeeProfileCompleted = true
        };

        var userResult = await userManager.CreateAsync(adminUser);
        if (!userResult.Succeeded)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Failed to create admin account. Error: {string.Join(", ", userResult.Errors.Select(x => x.Description))}");
            return;
        }

        var roleResult = await userManager.AddToRoleAsync(adminUser, "MasterAdmin");
        if (!roleResult.Succeeded)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Failed to add MasterAdmin role. Error: {string.Join(", ", roleResult.Errors.Select(x => x.Description))}");
            return;
        }

        var department = new Department
        {
            DepartmentName = departmentOptions.Value.DepartmentName
        };

        var position = new Position
        {
            PositionName = positionsOptions.Value.PositionName,
            Department = department
        };

        var employee = new Employee
        {
            PersonalNumber = employeeOptions.Value.PersonalNumber,
            FirstName = employeeOptions.Value.FirstName,
            LastName = employeeOptions.Value.LastName,
            ApplicationUserId = adminUser.Id,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Position = position
        };

        context.Employees.Add(employee);

        try
        {
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Failed to seed admin employee data. Error: {ex.Message}");
        }
    }

    private async Task SeedDefaultRolesAsync()
    {
        foreach (var role in roleOptions.Value)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                if (!roleResult.Succeeded)
                {
                    Console.WriteLine($"Create role: {role} failed. Error: {string.Join(", ", roleResult.Errors)}");
                }
            }
        }
    }

    private async Task<bool> ExistsAdminUserAsync()
    {
        var admin = await userManager.Users.FirstOrDefaultAsync(x => x.DomainUserName == userOptions.Value.DomainUserName);

        return admin is not null;
    }
}

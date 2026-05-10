using CompanyPortal.Domain.Entities;
using CompanyPortal.Infrastructure.AppUser;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Database;

public sealed class CompanyPortalDbContext(DbContextOptions<CompanyPortalDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Employee> Employees { get; set; } = default!;
    public DbSet<Position> Positions { get; set; } = default!;
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<Building> Buildings { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Position>(entity =>
        {
            entity.Property(x => x.PositionName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.HourlyRate)
                .HasColumnType("decimal(18,2)");

            entity.HasMany(x => x.Employees)
                .WithOne(x => x.Position)
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new { x.PositionName, x.DepartmentId }).IsUnique();
        }); 

        builder.Entity<Department>(entity =>
        {
            entity.Property(x => x.DepartmentName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasMany(x => x.Positions)
                .WithOne(x => x.Department)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.DepartmentName).IsUnique();
        });

        builder.Entity<Employee>(entity =>
        {
            entity.Property(x => x.PersonalNumber)
                .IsRequired()
                .HasMaxLength(16);

            entity.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.ApplicationUserId)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.HasIndex(x => x.PersonalNumber)
                .IsUnique();

            entity.HasIndex(x => x.ApplicationUserId)
                .IsUnique();

            entity.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Employee>(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(x => x.DomainUserName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.HasIndex(x => x.DomainUserName)
                .IsUnique();
        });

        builder.Entity<Building>(entity =>
        {
            entity.Property(x => x.BuildingName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.Description)
                .HasMaxLength(100);

            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(10);

            entity.HasMany(x => x.Departments)
                .WithOne(x => x.Building)
                .HasForeignKey(x => x.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.Code)
                .IsUnique();
        });
    }
}

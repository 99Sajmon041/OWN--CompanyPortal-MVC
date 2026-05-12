using CompanyPortal.Shared.Enums;

namespace CompanyPortal.Application.CurrentIdentity;

public interface ICurrentUserService
{
    string UserId { get; }
    string DomainUserName { get; }
    public int EmployeeId { get; }
    public int PositionId { get; }
    public int DepartmentId { get; }
    public PositionLevel? PositionLevel { get; }
    IReadOnlyCollection<string> Roles { get; }
    bool IsInRole(string role);
}
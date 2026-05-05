namespace CompanyPortal.Application.CurrentIdentity;

public interface ICurrentUserService
{
    string UserId { get; }
    string DomainUserName { get; }
    IReadOnlyCollection<string> Roles { get; }
    bool IsInRole(string role);
}
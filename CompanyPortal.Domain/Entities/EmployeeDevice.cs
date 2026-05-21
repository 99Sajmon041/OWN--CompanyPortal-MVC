namespace CompanyPortal.Domain.Entities;

public sealed class EmployeeDevice
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;
    public int DeviceId { get; set; }
    public Device Device { get; set; } = default!;
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnedAt { get; set; }
    public string? Note { get; set; }
}
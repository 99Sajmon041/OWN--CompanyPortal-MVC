namespace CompanyPortal.Application.Models.EmployeeDevice;

public sealed class EmployeeDeviceViewModel
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; } = default!;
    public int DeviceId { get; set; }
    public string DeviceName { get; set; } = default!;
    public DateTime AssignedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public string? Note { get; set; }
    public string SerialNumber { get; set; } = default!;
}

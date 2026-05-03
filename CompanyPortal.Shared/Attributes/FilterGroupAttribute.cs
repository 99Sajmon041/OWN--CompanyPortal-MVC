namespace CompanyPortal.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class FilterGroupAttribute(string groupName) : Attribute
{
    public string GroupName { get; } = groupName;
}

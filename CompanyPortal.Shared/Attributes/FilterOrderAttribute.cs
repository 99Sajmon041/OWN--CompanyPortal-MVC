namespace CompanyPortal.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class FilterOrderAttribute(int order) : Attribute
{
    public int Order { get; } = order;
}

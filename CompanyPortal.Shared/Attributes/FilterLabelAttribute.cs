namespace CompanyPortal.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class FilterLabelAttribute(string label) : Attribute
{
    public string Label { get; } = label;
}

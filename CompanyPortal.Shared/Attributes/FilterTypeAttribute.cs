using CompanyPortal.Shared.Grid.Filtering;

namespace CompanyPortal.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class FilterTypeAttribute(FilterInputType inputType) : Attribute
{
    public FilterInputType InputType { get; } = inputType;
}

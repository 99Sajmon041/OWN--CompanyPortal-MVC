using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CompanyPortal.Shared.Utilities;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());

        if (field is null)
        {
            return value.ToString();
        }

        DisplayAttribute? attribute = field.GetCustomAttribute<DisplayAttribute>();

        if (attribute is null)
        {
            return value.ToString();
        }

        return attribute.Name ?? value.ToString();
    }
}

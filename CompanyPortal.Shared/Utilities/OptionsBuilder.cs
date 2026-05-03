using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyPortal.Shared.Utilities;

public static class OptionsBuilder
{
    public static List<SelectListItem> GetPageSizeOptions()
    {
        return
        [
            new SelectListItem { Value = "5", Text = "5" },
            new SelectListItem { Value = "10", Text = "10" },
            new SelectListItem { Value = "20", Text = "20" },
            new SelectListItem { Value = "50", Text = "50" }
        ];
    }
}

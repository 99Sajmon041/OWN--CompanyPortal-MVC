using CompanyPortal.Shared.Grid.Paging;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPortal.Web.Views.ViewComponents;

public class PagerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(PagerPartialViewModel model)
    {
        return View(model);
    }
}

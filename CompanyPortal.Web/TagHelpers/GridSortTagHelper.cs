using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CompanyPortal.Web.TagHelpers;

[HtmlTargetElement("grid-sort")]
public sealed class GridSortTagHelper(IUrlHelperFactory urlHelperFactory) : TagHelper
{
    [HtmlAttributeName("property")]
    public string Property { get; set; } = default!;

    [HtmlAttributeName("label")]
    public string Label { get; set; } = default!;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = default!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Property);
        ArgumentException.ThrowIfNullOrWhiteSpace(Label);

        output.TagName = "th";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", "text-nowrap");

        var request = ViewContext.HttpContext.Request;
        var urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

        var ascendingRouteValues = CreateRouteValues(request, $"asc_{Property}");
        var descendingRouteValues = CreateRouteValues(request, $"desc_{Property}");

        string? ascendingUrl = urlHelper.Action(new UrlActionContext
        {
            Action = ViewContext.RouteData.Values["action"]?.ToString(),
            Controller = ViewContext.RouteData.Values["controller"]?.ToString(),
            Values = ascendingRouteValues
        });

        string? descendingUrl = urlHelper.Action(new UrlActionContext
        {
            Action = ViewContext.RouteData.Values["action"]?.ToString(),
            Controller = ViewContext.RouteData.Values["controller"]?.ToString(),
            Values = descendingRouteValues
        });

        output.Content.SetHtmlContent($$"""
            {{Label}}

            <a href="{{ascendingUrl}}"
               class="text-decoration-none text-nowrap"
               title="Seřadit vzestupně">
                <i class="bi bi-arrow-up"></i>
            </a>

            <a href="{{descendingUrl}}"
               class="text-decoration-none text-nowrap"
               title="Seřadit sestupně">
                <i class="bi bi-arrow-down"></i>
            </a>

            """);
    }

    private static Dictionary<string, string?> CreateRouteValues(HttpRequest request, string sortOrder)
    {
        var routeValues = request.Query
            .ToDictionary(x => x.Key, x => (string?)x.Value.ToString());

        routeValues["sortOrder"] = sortOrder;
        routeValues["page"] = "1";

        return routeValues;
    }
}

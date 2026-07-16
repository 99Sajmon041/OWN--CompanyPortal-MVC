using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Filtering;

namespace CompanyPortal.Web.RouteHelper;


public static class GridRequestBuilder
{
    public static GridRequest GetFromRequest(HttpRequest request)
    {
        var gridRequest = new GridRequest();

        if (int.TryParse(request.Query["page"], out int page))
        {
            gridRequest.Page = page;
        }

        if (int.TryParse(request.Query["pageSize"], out int pageSize))
        {
            gridRequest.PageSize = pageSize;
        }

        if (bool.TryParse(request.Query["filtersCollapsed"], out bool collapsed))
        {
            gridRequest.FiltersCollapsed = collapsed;
        }

        gridRequest.SortOrder = request.Query["sortOrder"];

        foreach (var item in request.Query)
        {
            if (item.Key == "page" ||
                item.Key == "pageSize" ||
                item.Key == "sortOrder" ||
                item.Key == "filtersCollapsed" ||
                item.Key.EndsWith("_Operator") ||
                item.Key == "selectedDate")
            {
                continue;
            }

            string propertyName = item.Key;
            string value = item.Value!;
            string operatorKey = $"{propertyName}_Operator";

            FilterOperator filterOperator = FilterOperator.Equal;

            if (Enum.TryParse(request.Query[operatorKey], out FilterOperator parsedOperator))
            {
                filterOperator = parsedOperator;
            }

            string propertyPath;

            bool from = propertyName.EndsWith("_From");
            bool to = propertyName.EndsWith("_To");

            if (from || to)
            {
                filterOperator = from ? FilterOperator.GreaterThanOrEqual : FilterOperator.LessThanOrEqual;

                int index = propertyName.LastIndexOf('_');
                propertyPath = propertyName.Substring(0, index);

                if (propertyPath.Contains('_'))
                {
                    propertyPath = propertyPath.Replace('_', '.');
                }
            }
            else
            {
                propertyPath = propertyName.Contains('_') ? propertyName.Replace('_', '.') : propertyName;
            }

            gridRequest.Filters.Add(new FilterItem
            {
                PropertyName = propertyName,
                PropertyPath = propertyPath,
                Value = value,
                Operator = filterOperator
            });
        }

        return gridRequest;
    }
}

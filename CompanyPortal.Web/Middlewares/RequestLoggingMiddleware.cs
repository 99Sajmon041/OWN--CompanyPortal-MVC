using System.Diagnostics;
using Serilog;
using Serilog.Context;

namespace CompanyPortal.Web.Middlewares;

public sealed class RequestLoggingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        string userName = context.User.Identity?.Name ?? "Anonymous";
        string endpoint = context.Request.Path;
        string method = context.Request.Method;
        string queryString = context.Request.QueryString.ToString();

        using (LogContext.PushProperty("User", userName))
        using (LogContext.PushProperty("Endpoint", endpoint))
        using (LogContext.PushProperty("Method", method))
        using (LogContext.PushProperty("QueryString", queryString))
        {
            await next(context);

            stopwatch.Stop();

            Log.Information("Request {Method} {Endpoint} completed with status {StatusCode} in {ElapsedMilliseconds} ms. User: {User}. Query: {QueryString}",
                method,
                endpoint,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                userName,
                queryString);
        }
    }
}

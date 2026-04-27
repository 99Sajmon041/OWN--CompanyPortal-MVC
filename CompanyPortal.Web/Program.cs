using Microsoft.AspNetCore.Authentication.Negotiate;
using CompanyPortal.Application.Extentions;
using CompanyPortal.Infrastructure.Extentions;
using CompanyPortal.Web.Middlewares;
using Microsoft.AspNetCore.DataProtection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var keysDirectory = new DirectoryInfo(@"C:\AppKeys\CompanyPortal");

if (!keysDirectory.Exists)
{
    keysDirectory.Create();
}

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(keysDirectory)
    .SetApplicationName("CompanyPortal");

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapStaticAssets();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

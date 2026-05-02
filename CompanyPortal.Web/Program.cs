using CompanyPortal.Application.Extentions;
using CompanyPortal.Infrastructure.AppUser;
using CompanyPortal.Infrastructure.Database;
using CompanyPortal.Infrastructure.Extentions;
using CompanyPortal.Web.Middlewares;
using CompanyPortal.Web.Seed;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
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
builder.Services.AddScoped<DefaultSeeder>();
builder.Services.AddScoped<ApplicationUserClaimsMiddleware>();
builder.Services.AddScoped<EmployeeProfileRequiredMiddleware>();

var app = builder.Build();

app.UseExceptionHandler("/Error/Unexpected");

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

using var scope = app.Services.CreateScope();
{
    var seeder = scope.ServiceProvider.GetRequiredService<DefaultSeeder>();
    await seeder.SeedAdminUserAsync();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStatusCodePagesWithReExecute("/Error/StatusCode", "?code={0}");

app.UseAuthentication();
app.UseMiddleware<ApplicationUserClaimsMiddleware>();
app.UseMiddleware<EmployeeProfileRequiredMiddleware>();
app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapStaticAssets();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

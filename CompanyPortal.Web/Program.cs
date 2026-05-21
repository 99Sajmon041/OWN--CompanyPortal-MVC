using CompanyPortal.Application.CurrentIdentity;
using CompanyPortal.Application.Extentions;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Infrastructure.Extentions;
using CompanyPortal.Web.CurrentIdentity;
using CompanyPortal.Web.Middlewares;
using CompanyPortal.Web.Seed;
using CompanyPortal.Web.ServicesImplementations;
using Microsoft.AspNetCore.Authentication.Negotiate;
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
builder.Services.AddScoped<DefaultSeeder>();
builder.Services.AddScoped<ApplicationUserClaimsMiddleware>();
builder.Services.AddScoped<EmployeeProfileRequiredMiddleware>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IImageService, ImageService>();

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
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthentication();
app.UseMiddleware<ApplicationUserClaimsMiddleware>();
app.UseMiddleware<EmployeeProfileRequiredMiddleware>();
app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}")
    .WithStaticAssets();

app.Run();

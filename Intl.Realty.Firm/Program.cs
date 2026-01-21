using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Models.Auxiliary;
using Intl.Realty.Firm.Repository;
using Intl.Realty.Firm.Repository.IRepository;
using Intl.Realty.Firm.Security;
using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Intl.Realty.Firm.Service.Export;
using Intl.Realty.Firm.Service.IService.IExport;
using Newtonsoft.Json.Serialization;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Models;

using Serilog;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Load secrets
builder.Configuration.AddUserSecrets<Program>();

// Set non-commercial license for EPPlus
ExcelPackage.License.SetNonCommercialPersonal("Kyle Bouchard");

builder.Host.UseSerilog();

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Google CAPTCHA config
builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleReCaptcha"));

// Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 2;
    options.Password.RequireNonAlphanumeric = false;

    options.SignIn.RequireConfirmedEmail = true;
    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

// Token lifespan configs
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromHours(5));
builder.Services.Configure<CustomEmailConfirmationTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromDays(3));

// MVC & JSON
builder.Services
    .AddControllersWithViews(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    })
    .AddXmlSerializerFormatters()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
    });

// Application cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
});

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CreateRolePolicy", policy =>
        policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

    options.AddPolicy("DeleteRolePolicy", policy =>
        policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

    options.AddPolicy("EditRolePolicy", policy =>
        policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

    options.AddPolicy("AdminRolePolicy", policy =>
        policy.RequireRole("Admin", "DealAdmin"));
});

// Service Registrations
builder.Services.AddTransient(typeof(IExportService<>), typeof(ExportService<>));
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<ICsvService, CsvService>();
builder.Services.AddScoped<IHtmlService, HtmlService>();
builder.Services.AddScoped<IJsonService, JsonService>();
builder.Services.AddScoped<IXmlService, XmlService>();
builder.Services.AddScoped<IYamlService, YamlService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAllDealsService, AllDealsService>();
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
builder.Services.AddSingleton<DataProtectionPurposeStrings>();

builder.Services.AddTransient<IConfigurationService, ConfigurationService>();
builder.Services.AddTransient<IFileHandlerService, FileHandlerService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IDocumentTransactionService, DocumentTransactionService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IGoogleCaptchaService, GoogleCaptchaService>();
builder.Services.AddScoped<IDealTransactionNumberService, DealTransactionNumberService>();

// Form upload limits
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MultipartBoundaryLengthLimit = int.MaxValue;
    o.MultipartHeadersCountLimit = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
    o.BufferBodyLengthLimit = int.MaxValue;
    o.BufferBody = true;
    o.ValueCountLimit = int.MaxValue;
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});

builder.WebHost.ConfigureKestrel(c =>
{
    c.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(100);
});

// Email settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var emailConfig = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
Log.Information("📧 Email Host: {Host}, Port: {Port}, User: {User}, Password Exists: {HasPassword}",
    emailConfig?.Host, emailConfig?.Port, emailConfig?.Email,
    !string.IsNullOrWhiteSpace(emailConfig?.Password));

// -------------------- Build App --------------------
var app = builder.Build();

// -------------------- Middleware --------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Handle status codes (e.g., 404, 403)
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/uploads"
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Endpoint to receive Core Web Vitals (CWV) data

app.MapPost("/cwv", async (HttpContext ctx) =>
{
    using var reader = new StreamReader(ctx.Request.Body);
    var json = await reader.ReadToEndAsync();

    ctx.RequestServices
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("CWV")
        .LogInformation("CWV: {Json}", json);

    return Results.Ok(new { ok = true });
})
.AllowAnonymous();



app.Run();

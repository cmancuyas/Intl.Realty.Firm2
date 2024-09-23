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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(name: "DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequireNonAlphanumeric = false;

    options.SignIn.RequireConfirmedEmail = true;
    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromHours(5));

builder.Services.Configure<CustomEmailConfirmationTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromDays(3));

builder.Services.AddMvc(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddXmlSerializerFormatters();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
});

builder.Services.AddAuthorization(options =>
{
    // Claims Policy
    options.AddPolicy("CreateRolePolicy",
        policy => policy
        //.RequireClaim("Create Role", "true"));
        .AddRequirements(new ManageAdminRolesAndClaimsRequirement())

        );

    options.AddPolicy("DeleteRolePolicy",
            policy => policy
            //.RequireClaim("Delete Role", "true"));
            .AddRequirements(new ManageAdminRolesAndClaimsRequirement())

            );

    options.AddPolicy("EditRolePolicy",
            policy => policy

            .AddRequirements(new ManageAdminRolesAndClaimsRequirement())

            //.RequireAssertion(context =>
            //context.User.IsInRole("SuperAdmin") || context.User.IsInRole("Admin") ||
            //context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true" &&
            //context.User.IsInRole("DealAdmin") 
            //))

            //.RequireClaim("Edit Role", "true")
            //.RequireRole("Admin")
            //.RequireRole("DealAdmin")
            //.RequireRole("SuperAdmin")
            );
    //options.InvokeHandlersAfterFailure = false;
    //Roles Policy 
    options.AddPolicy("AdminRolePolicy",
        policy => policy
                .RequireRole("Admin", "DealAdmin"));
});

builder.Services.AddTransient(typeof(IExportService<>), typeof(ExportService<>));
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<ICsvService, CsvService>();
builder.Services.AddScoped<IHtmlService, HtmlService>();
builder.Services.AddScoped<IJsonService, JsonService>();
builder.Services.AddScoped<IXmlService, XmlService>();
builder.Services.AddScoped<IYamlService, YamlService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
builder.Services.AddSingleton<DataProtectionPurposeStrings>();

builder.Services.AddTransient<IConfigurationService, ConfigurationService>();
builder.Services.AddTransient<IFileHandlerService, FileHandlerService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IDocumentTransactionService, DocumentTransactionService>();

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
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddMvc().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error");
    //app.UseStatusCodePagesWithRedirects("/Error/{0}");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // identifying who the user is
app.UseAuthorization(); // identifying what the user can do

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

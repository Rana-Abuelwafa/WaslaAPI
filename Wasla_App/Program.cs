using Mails_App;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Wasla_App.services;
using Wasla_App.services.Admin;
using Wasla_App.services.Client;
using Wasla_App.Services;
using WaslaApp.Data;
using WaslaApp.Data.Data;
using WaslaApp.Data.Entities;
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
// Configure QuestPDF license
QuestPDF.Settings.License = LicenseType.Community;
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
// Add services to the container.
//builder.Services.AddAuthentication().AddGoogle(googleOptions =>
//{
//    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//});
//builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddRazorPages();
builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);
//builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("*")
                    .WithMethods("*")
                    .WithHeaders(HeaderNames.ContentType, "*");
        });
});
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(MyAllowSpecificOrigins, policy =>
//    {
//        policy
//            .WithOrigins("https://localhost:3000", "https://localhost:3001", "https://waslaa.de/") // React dev server
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .AllowCredentials(); // allow cookies
//    });
//});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
    options.OperationFilter<AcceptLanguageHeaderOperationFilter>();
});
builder.Services.AddHttpContextAccessor();
//mail
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddDbContext<wasla_client_dbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WaslaConnection")));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<WaslaDAO>();
builder.Services.AddScoped<WaslaAdminDao>();
builder.Services.AddScoped<MailSettingDao>();
builder.Services.AddScoped<IWaslaService, WaslaService>();
builder.Services.AddScoped<IAdminWaslaService, AdminWaslaService>();
builder.Services.AddScoped<CustomViewRendererService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "ar", "de" }; // Add more as needed
    options.SetDefaultCulture("en");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//localization
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

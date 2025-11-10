using Mails_App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using Wasla_Auth_App;
using Wasla_Auth_App.Models;
using Wasla_Auth_App.Services;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
// Add services to the container.

builder.Services.AddControllers();
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
//mail
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AUTH API", Version = "v1" });

    // Add Accept-Language header globally
    c.OperationFilter<AcceptLanguageHeaderOperationFilter>();
});

builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddCookie(IdentityConstants.ApplicationScheme)
////.AddBearerToken(IdentityConstants.BearerScheme)
//.AddJwtBearer(options =>
//{
//    options.SaveToken = true;
//    options.RequireHttpsMetadata = false;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        //ValidAudience = Configuration["JWT:ValidAudience"],
//        //ValidIssuer = Configuration["JWT:ValidIssuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetConnectionString("JWT:Secret")))

//    };
//});


builder.Services.AddIdentityCore<ApplicationUser>()
     .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthenticationDBContext>()
     .AddDefaultTokenProviders()
    .AddApiEndpoints();

builder.Services.AddDbContext<AuthenticationDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthConnection")));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthenticationDBContext>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddScoped<MailSettingDao>();

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
app.UseHttpsRedirection();
//localization
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);
app.UseMiddleware<LocalizedExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.MapIdentityApi<ApplicationUser>();
app.MapControllers();

app.Run();

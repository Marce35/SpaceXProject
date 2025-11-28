using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SpaceXProject.api.Data.Context;
using SpaceXProject.api.Data.Models.Authentication;
using SpaceXProject.api.ExternalApiClient;
using SpaceXProject.api.ExternalApiClient.Interfaces;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SpaceXProject.api.Configuration;
using SpaceXProject.api.Data.Security;
using SpaceXProject.api.Data.Security.JwtToken;
using SpaceXProject.api.Extensions;
using SpaceXProject.api.Shared.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddHttpClient<IExternalApiClient, ExternalApiClient>((sp, client) =>
{
    client.BaseAddress = new Uri("https://api.spacexdata.com/v5/");
});


builder.Services
    .AddDbContext<ApplicationContext>(options => options
        .UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"))
        .LogTo(Console.WriteLine));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        string[]? allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
        if (allowedOrigins is not null && allowedOrigins.Any())
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
        else
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    });
});

builder.Services.Configure<IdentityEncryptionKeyConfig>(builder.Configuration.GetSection("IdentityEncryptionKeys"));

builder.Services.AddApplicationServices();

builder.Services.AddAuthorization();

builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequiredLength = ApplicationConstants.MinPasswordLength;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;

        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = true;

        options.User.RequireUniqueEmail = true;

        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedAccount = false;


        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 3;

        options.Stores.ProtectPersonalData = true;

    })
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders()
    .AddPersonalDataProtection<IdentityLookupProtector, IdentityLookupProtectorKeyRing>();


#region JWT token implementation

var jwtSection = builder.Configuration.GetSection("jwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);

var jwtSettings = jwtSection.Get<JwtSettings>();

if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
{
    throw new InvalidOperationException("JwtSettings are not configured properly. Check User Secrets.");
}

var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                {
                    context.Token = context.Request.Cookies["X-Access-Token"];
                }

                return Task.CompletedTask;
            }
        };
    });


#endregion



var app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("SpaceX Project API Documentation");
        options.WithTheme(ScalarTheme.DeepSpace);
    });

    ApplicationContext context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseCors("AllowedOrigins");


app.UseAuthentication();

app.MapControllers();

app.Run();

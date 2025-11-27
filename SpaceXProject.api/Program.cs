using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SpaceXProject.api.Data.Context;
using SpaceXProject.api.Data.Models.Authentication;
using SpaceXProject.api.ExternalApiClient;
using SpaceXProject.api.ExternalApiClient.Interfaces;
using System.Text.Json.Serialization;

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


builder.Services.AddAuthorization();

builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
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

    })
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();



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

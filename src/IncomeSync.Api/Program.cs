using System.Globalization;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using IncomeSync.Api.Middlewares.JwtTokenMiddleware;
using IncomeSync.Api.Middlewares.UserExceptionMiddleware;
using IncomeSync.Api.Middlewares.ValidationMiddleware;
using IncomeSync.Api.Validations;
using IncomeSync.Api.Validations.UserValidation;
using IncomeSync.Core;
using IncomeSync.Core.Services.UserService;
using IncomeSync.Persistence;
using IncomeSync.Persistence.Repositories.UserRepository;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IncomeSync.Api;

internal static class Program
{
    private static readonly IConfigurationRoot ConfigurationRoot = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        AddServices(builder);
        AddDbContext(builder);
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseMiddleware<ValidationExceptionMiddleware>();
        app.UseMiddleware<JwtTokenMiddleware>();
        app.UseMiddleware<UserExceptionMiddleware>();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    private static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(_ =>
            {
                _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = ConfigurationRoot["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = ConfigurationRoot["JwtSettings:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationRoot["JwtSettings:PrivateKey"]!)),
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });
        
        builder.Services.AddControllers();
        builder.Services.AddValidatorsFromAssemblies(new[] { typeof(Program).Assembly });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMediatR(_ => _.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        
        builder.Services.AddValidatorsFromAssembly(IncomeSyncCoreAsync.Value);
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddAuthorization();
        
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
    }

    private static void AddDbContext(WebApplicationBuilder builder)
    { 
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(ConfigurationRoot.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)));
    }
}
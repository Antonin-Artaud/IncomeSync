using System.Security.Cryptography;
using FluentValidation;
using IncomeSync.Api.Middlewares.UserExceptionMiddleware;
using IncomeSync.Api.Validations;
using IncomeSync.Api.Validations.UserValidation;
using IncomeSync.Core;
using IncomeSync.Core.Providers.KeysProvider;
using IncomeSync.Core.Services.UserService;
using IncomeSync.Core.Shared;
using IncomeSync.Core.Shared.Contracts.Requests.AuthRequest;
using IncomeSync.Core.Shared.Exceptions;
using IncomeSync.Persistence;
using IncomeSync.Persistence.Repositories.UserRepository;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
        }).AddJwtBearer(ConfigureJwtOptions);
        
        builder.Services.AddControllers();
        builder.Services.AddValidatorsFromAssemblies(new[] { typeof(Program).Assembly });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "IncomeSync API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        
        builder.Services.AddMediatR(_ =>
        {
            _.RegisterServicesFromAssembly(typeof(Program).Assembly);
            _.RegisterServicesFromAssembly(typeof(IncomeSyncCoreAsync).Assembly);
            _.RegisterServicesFromAssembly(typeof(IncomeSyncCoreSharedAsync).Assembly);
        });
        builder.Services.AddAuthorization();
        
        AddProviders(builder);
        AddValidators(builder);
        AddCoreServices(builder);
        AddRepositories(builder);
    }

    private static void AddRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddCoreServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserService, UserService>();
    }

    private static void AddValidators(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<CreateLogInRequest>, CreateLogInRequestValidator>();
        builder.Services.AddScoped<IValidator<CreateAccountRequest>, CreateAccountRequestValidator>();
    }

    private static void AddProviders(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPublicKeyProvider, PublicKeyProvider>();
        builder.Services.AddSingleton<IPrivateKeyProvider, PrivateKeyProvider>();
    }

    private static async void ConfigureJwtOptions(JwtBearerOptions options)
    {
        var publicKey = await File.ReadAllTextAsync(ConfigurationRoot["JwtSettings:PublicKey"] ?? throw new PublicKeyNotFoundException());
        var ecdsa = ECDsa.Create();
        ecdsa.ImportFromPem(publicKey);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new ECDsaSecurityKey(ecdsa),
            ValidateIssuer = true,
            ValidIssuer = ConfigurationRoot["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = ConfigurationRoot["JwtSettings:Audience"],
        };
    }

    private static void AddDbContext(WebApplicationBuilder builder)
    { 
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(ConfigurationRoot.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)));
    }
}
using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using IncomeSync.Api.Middlewares.UserExceptionMiddleware;
using IncomeSync.Api.Middlewares.ValidationMiddleware;
using IncomeSync.Api.Validations;
using IncomeSync.Core;
using IncomeSync.Core.Services.UserService;
using IncomeSync.Persistence;
using IncomeSync.Persistence.Repositories.UserRepository;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

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
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

                // Activer l'authentification par jeton JWT
                c.DocExpansion(DocExpansion.None);
            });
        }
        
        app.UseMiddleware<ValidationExceptionMiddleware>();
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
                var publicKey = File.ReadAllText(ConfigurationRoot["JwtSettings:PublicKeyFilePath"] ?? throw new FileNotFoundException());
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
            });
        
        builder.Services.AddControllers();
        builder.Services.AddValidatorsFromAssemblies(new[] { typeof(Program).Assembly });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
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
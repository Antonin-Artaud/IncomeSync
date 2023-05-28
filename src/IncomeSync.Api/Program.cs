using System.Globalization;
using FluentValidation;
using IncomeSync.Api.Middlewares.UserExceptionMiddleware;
using IncomeSync.Api.Middlewares.ValidationMiddleware;
using IncomeSync.Api.Validations;
using IncomeSync.Core;
using IncomeSync.Core.Services.UserService;
using IncomeSync.Persistence;
using IncomeSync.Persistence.Repositories.UserRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IncomeSync.Api;

internal static class Program
{
    private static IConfigurationRoot _configurationRoot =
        new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    
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
        app.UseMiddleware<UserExceptionMiddleware>();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
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
            options.UseSqlServer(_configurationRoot.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)));
    }
}
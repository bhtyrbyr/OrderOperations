using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrderOperations.Application.TokenOperations;
using System.Reflection;
using System.Text;

namespace OrderOperations.Application;

public static class ServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var collection = services;
        var assm = Assembly.GetExecutingAssembly();

        // MediatR
        collection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assm));

        // FluentValidation
        services.AddValidatorsFromAssembly(assm);

        // AutoMapper
        collection.AddAutoMapper(assm);

        // JWT Authentication
        collection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
#pragma warning disable CS8604 // Olası null başvuru bağımsız değişkeni.
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
#pragma warning restore CS8604 // Olası null başvuru bağımsız değişkeni.
                ClockSkew = TimeSpan.Zero
            };
        });

        collection.AddScoped<IAuthService, AuthService>();

        return services;
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrderOperations.Application.TokenOperations;
using OrderOperations.Security.TokenOperations;
using System.Text;

namespace OrderOperations.Security;

public static class ServicesRegistration
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {

        var collection = services;
        var validIssuer = configuration["JWT:Issuer"];
        var validAudience = configuration["JWT:Audience"];
        var validSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

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
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = validSigningKey,
                ClockSkew = TimeSpan.Zero
            };
        });

        collection.AddScoped<IAuthService, AuthService>();
        return collection;
    }
}

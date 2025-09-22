using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderOperations.Application.TokenOperations;
using System.Text;

namespace OrderOperations.Application;

public static class ServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var collection = services;
        var assm = Assembly.GetExecutingAssembly();

        // MediatR
        collection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assm));

        // FluentValidation
        services.AddValidatorsFromAssembly(assm);

        // AutoMapper
        collection.AddAutoMapper(assm);

        return services;
    }
}

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnackHub.OrderService.Api.Configuration;
using SnackHub.OrderService.Application.EventConsumers.Client;
using SnackHub.OrderService.Application.EventConsumers.Payment;
using SnackHub.OrderService.Application.EventConsumers.Production;

namespace SnackHub.OrderService.Api.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var settings = configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
        
        serviceCollection.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<ClientAddedConsumer>();
            busConfigurator.AddConsumer<ClientRemovedConsumer>();
            
            busConfigurator.AddConsumer<PaymentApprovedConsumer>();
            busConfigurator.AddConsumer<PaymentRejectedConsumer>();
            
            busConfigurator.AddConsumer<ProductionOrderAcceptedConsumer>();
            busConfigurator.AddConsumer<ProductionOrderCompletedConsumer>();
            
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                // configurator.ReceiveEndpoint(settings.Host, queueConfigurator =>
                // {
                //     queueConfigurator.AutoDelete = true;
                // });
                
                configurator.Host(settings.Host, "/",  rabbitMqHostConfigurator =>
                {
                    rabbitMqHostConfigurator.Username(settings.UserName);
                    rabbitMqHostConfigurator.Password(settings.Password);							
                });
                
                configurator.AutoDelete = true;
                configurator.ConfigureEndpoints(context);
            });
            
        });
        
        return serviceCollection;
    }
}
using Microsoft.Extensions.DependencyInjection;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Infra.Repositories.MongoDB;

namespace SnackHub.OrderService.Api.Extensions;

public static class RepositoriesExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<IClientRepository, ClientRepository>()
            .AddScoped<IProductRepository, ProductRepository>();

    }
}
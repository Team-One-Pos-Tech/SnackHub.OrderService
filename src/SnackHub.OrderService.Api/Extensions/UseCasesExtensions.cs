using Microsoft.Extensions.DependencyInjection;
using SnackHub.OrderService.Application.Order.Contracts;
using SnackHub.OrderService.Application.Order.UseCases;

namespace SnackHub.OrderService.Api.Extensions;

public static class UseCasesExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IConfirmOrderUseCase, ConfirmOrderUseCase>()
            .AddScoped<ICancelOrderUseCase, CancelOrderUseCase>()
            .AddScoped<IListOrderUseCase, ListOrderUseCase>();
    }
}
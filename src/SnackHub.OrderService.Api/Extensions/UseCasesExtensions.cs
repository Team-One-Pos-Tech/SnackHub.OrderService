using Microsoft.Extensions.DependencyInjection;
using SnackHub.OrderService.Application.Contracts;
using SnackHub.OrderService.Application.UseCases;

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
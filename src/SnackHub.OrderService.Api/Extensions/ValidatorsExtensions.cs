using Microsoft.Extensions.DependencyInjection;

namespace SnackHub.OrderService.Api.Extensions;

public static class ValidatorsExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
}
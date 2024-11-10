using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SnackHub.OrderService.Application.Models.Product;
using SnackHub.OrderService.Domain.Contracts;

namespace SnackHub.OrderService.Application.EventConsumers.Product;

public class ProductDeletedConsumer : IConsumer<ProductDeleted>
{
    private readonly ILogger<ProductDeletedConsumer> _logger;
    private readonly IProductRepository _productRepository;

    public ProductDeletedConsumer(
        ILogger<ProductDeletedConsumer> logger, 
        IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task Consume(ConsumeContext<ProductDeleted> context)
    {
        _logger.LogInformation("The product with id [{productId}] has been removed by a external service", context.Message.Id);
        await _productRepository.RemoveAsync(context.Message.Id);
    }
}
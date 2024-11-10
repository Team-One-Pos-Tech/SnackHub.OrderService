using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SnackHub.OrderService.Application.Models.Product;
using SnackHub.OrderService.Domain.Contracts;

namespace SnackHub.OrderService.Application.EventConsumers.Product;

public class ProductCreatedConsumer : IConsumer<ProductCreated>
{
    private readonly ILogger<ProductCreatedConsumer> _logger;
    private readonly IProductRepository _productRepository;

    public ProductCreatedConsumer(
        ILogger<ProductCreatedConsumer> logger, 
        IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task Consume(ConsumeContext<ProductCreated> context)
    {
        _logger.LogInformation("The product [{productName}] has been created by a external service", context.Message.Name);
        await _productRepository.RemoveAsync(context.Message.Id);
    }
}
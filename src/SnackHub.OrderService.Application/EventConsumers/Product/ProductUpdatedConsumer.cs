using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SnackHub.OrderService.Application.Models.Product;
using SnackHub.OrderService.Domain.Contracts;

namespace SnackHub.OrderService.Application.EventConsumers.Product;

public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
{
    private readonly ILogger<ProductUpdatedConsumer> _logger;
    private readonly IProductRepository _productRepository;

    public ProductUpdatedConsumer(
        ILogger<ProductUpdatedConsumer> logger, 
        IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task Consume(ConsumeContext<ProductUpdated> context)
    {
        _logger.LogInformation("The product with id [{productId}] has been updated by a external service!", context.Message.Id);
        
        var product = await _productRepository.GetProductByIdAsync(context.Message.Id);
        if (product == null)
        {
            _logger.LogInformation("The product with id [{productId}] not found!", context.Message.Id);
            return;   
        }

        product.Edit(
            context.Message.Name,
            context.Message.Description,
            context.Message.Price);
        
        await _productRepository.EditAsync(product);
    }
    
}
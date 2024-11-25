using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using SnackHub.OrderService.Application.Contracts;
using SnackHub.OrderService.Application.Models.Order;
using SnackHub.OrderService.Application.Models.Payment;
using SnackHub.OrderService.Domain.Base;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.Entities;
using SnackHub.OrderService.Domain.ValueObjects;
using OrderFactory = SnackHub.OrderService.Domain.Entities.Order.Factory;
using OrderItemFactory = SnackHub.OrderService.Domain.ValueObjects.OrderItem.Factory;

namespace SnackHub.OrderService.Application.UseCases;

public class ConfirmOrderUseCase : IConfirmOrderUseCase 
{
    private readonly IOrderRepository _orderRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IProductRepository _productRepository;
    
    private readonly IPublishEndpoint _publishEndpoint;
    
    public ConfirmOrderUseCase(
        IOrderRepository orderRepository, 
        IClientRepository clientRepository,
        IProductRepository productRepository,
        IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _clientRepository = clientRepository;
        _productRepository = productRepository;
        
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<ConfirmOrderResponse> Execute(ConfirmOrderRequest request)
    {
        var response = new ConfirmOrderResponse();
        
        var client = await GetClient(request.Identifier);
        if (client is null)
        {
            response.AddNotification(nameof(request.Identifier), "Client not found");
            return response;
        }
        
        var (orderItems, orderItemsValid) = await GetOrderItems(request.Items);
        if (!orderItemsValid)
        {
            response.AddNotification(nameof(request.Items), "One or more products could not be found");
            return response;
        }

        try
        {
            return await ConfirmOrderAsync(client, orderItems);
        } 
        catch (DomainException e)
        {
            response.AddNotification("Order", e.Message);
            return response;
        }
    }

    private async Task<Client?> GetClient(string identifier)
    {
        if (Guid.TryParse(identifier, out var clientId))
            return   await _clientRepository.GetByIdentifierAsync(clientId);
        
        return null;
    }
    
    private async Task<(IReadOnlyCollection<OrderItem>, bool)> GetOrderItems(IEnumerable<ConfirmOrderRequest.Item> items)
    {
        var requestItems = items
            .GroupBy(item => item.ProductId)
            .ToDictionary(item => item.Key, group => group.Sum(g => g.Quantity));
        
        var products = await _productRepository.GetByIdsAsync(requestItems.Keys);
        var productMap = products.ToDictionary(product => product.Id);
        if (productMap.Count != requestItems.Count)
        {
            return ([], false);
        }
        
        var orderItems = requestItems
            .Select(item =>
            {
                var product = productMap[item.Key];
                return OrderItemFactory.Create(product.Id, product.Name, product.Price, item.Value);
            })
            .ToList()
            .AsReadOnly();

        return (orderItems, true);
    }

    private async Task<ConfirmOrderResponse> ConfirmOrderAsync(Client client, IReadOnlyCollection<OrderItem> orderItems)
    {
        var order = OrderFactory.Create(client.Identifier, orderItems);
        order.Confirm();

        var response = new ConfirmOrderResponse
        {
            OrderId = order.Id,
            Total = order.Total,
            CreatedAt = order.CreatedAt
        };

        await _orderRepository.AddAsync(order);
            
        var paymentRequest = new PaymentRequest(order.Id, order.ClientId, order.Total);
        await _publishEndpoint.Publish(paymentRequest);

        return response;
    }
}

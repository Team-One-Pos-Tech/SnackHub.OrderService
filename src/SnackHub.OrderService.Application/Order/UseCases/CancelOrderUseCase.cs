using System.Threading.Tasks;
using SnackHub.OrderService.Application.Order.Contracts;
using SnackHub.OrderService.Application.Order.Models.Order;
using SnackHub.OrderService.Domain.Base;
using SnackHub.OrderService.Domain.Contracts;

namespace SnackHub.OrderService.Application.Order.UseCases;

public class CancelOrderUseCase : ICancelOrderUseCase 
{
    private readonly IOrderRepository _orderRepository;
    
    public CancelOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<CancelOrderResponse> Execute(CancelOrderRequest request)
    {
        var response = new CancelOrderResponse();
        
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order is null)
        {
            response.AddNotification(nameof(request.OrderId), "Order not found");
            return response;
        }

        try
        {
            order.Cancel();
            
            await _orderRepository.EditAsync(order);
            
            response.CancelledAt = order.UpdatedAt;
        }
        catch (DomainException e)
        {
            response.AddNotification(nameof(request.OrderId), e.Message);
        }
        
        return response;
    }
}

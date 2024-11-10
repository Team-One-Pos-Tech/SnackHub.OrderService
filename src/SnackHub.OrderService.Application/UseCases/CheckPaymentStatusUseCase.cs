using System.Threading.Tasks;
using SnackHub.OrderService.Application.Contracts;
using SnackHub.OrderService.Application.Models.Order;
using SnackHub.OrderService.Domain.Contracts;

namespace SnackHub.OrderService.Application.UseCases
{
    public class CheckPaymentStatusUseCase : ICheckPaymentStatusUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public CheckPaymentStatusUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CheckPaymentStatusResponse> Execute(CheckPaymentStatusRequest request)
        {
            var response = new CheckPaymentStatusResponse();
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                response.AddNotification("Order", "Order not found.");
                return response;
            }

            response.PaymentStatus = order.Status;
            return response;
        }
    }

}

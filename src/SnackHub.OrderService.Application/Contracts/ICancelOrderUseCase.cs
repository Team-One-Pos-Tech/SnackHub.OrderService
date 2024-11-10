using System.Threading.Tasks;
using SnackHub.OrderService.Application.Models.Order;

namespace SnackHub.OrderService.Application.Contracts;

public interface ICancelOrderUseCase
{
    Task<CancelOrderResponse> Execute(CancelOrderRequest request);
}
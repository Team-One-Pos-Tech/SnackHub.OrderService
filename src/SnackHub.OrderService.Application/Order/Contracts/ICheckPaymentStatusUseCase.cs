using System.Threading.Tasks;
using SnackHub.OrderService.Application.Order.Models.Order;

namespace SnackHub.OrderService.Application.Order.Contracts
{
    public interface ICheckPaymentStatusUseCase
    {
        Task<CheckPaymentStatusResponse> Execute(CheckPaymentStatusRequest request);
    }

}

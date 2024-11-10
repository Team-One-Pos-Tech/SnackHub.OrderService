using System.Threading.Tasks;
using SnackHub.OrderService.Application.Models.Order;

namespace SnackHub.OrderService.Application.Contracts
{
    public interface ICheckPaymentStatusUseCase
    {
        Task<CheckPaymentStatusResponse> Execute(CheckPaymentStatusRequest request);
    }

}

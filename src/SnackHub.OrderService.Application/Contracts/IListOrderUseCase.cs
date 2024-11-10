using System.Collections.Generic;
using System.Threading.Tasks;
using SnackHub.OrderService.Application.Models.Order;

namespace SnackHub.OrderService.Application.Contracts;

public interface IListOrderUseCase
{
    Task<IEnumerable<OrderResponse>> Execute();   
}
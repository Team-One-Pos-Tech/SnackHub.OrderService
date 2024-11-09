using System.Collections.Generic;
using System.Threading.Tasks;
using SnackHub.OrderService.Application.Order.Models.Order;

namespace SnackHub.OrderService.Application.Order.Contracts;

public interface IListOrderUseCase
{
    Task<IEnumerable<OrderResponse>> Execute();   
}
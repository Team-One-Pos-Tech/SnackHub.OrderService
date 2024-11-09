using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnackHub.OrderService.Domain.Entities;

namespace SnackHub.OrderService.Domain.Contracts;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task EditAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> ListAllAsync();
}
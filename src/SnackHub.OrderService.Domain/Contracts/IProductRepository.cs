using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnackHub.OrderService.Domain.Entities;

namespace SnackHub.OrderService.Domain.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task AddAsync(Product product);
        Task EditAsync(Product product);
        Task RemoveAsync(Guid id);
    }
}

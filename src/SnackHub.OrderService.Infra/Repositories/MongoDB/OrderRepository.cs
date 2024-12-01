using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.Entities;

namespace SnackHub.OrderService.Infra.Repositories.MongoDB;

public sealed class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(
        IMongoDatabase mongoDatabase,
        string collectionName = "Orders") 
        : base(mongoDatabase, collectionName)
    {
    }
    
    public async Task AddAsync(Order order)
    {
        await InsertAsync(order);
    }
    
    public async Task EditAsync(Order order)
    {
        await UpdateByPredicateAsync(x => x.Id.Equals(order.Id), order);
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await FindByPredicateAsync(x => x.Id.Equals(id));
    }
    
    public async Task<IEnumerable<Order>> ListAllAsync()
    {
        return await ListByPredicateAsync(px => true) ?? ArraySegment<Order>.Empty;
    }
}
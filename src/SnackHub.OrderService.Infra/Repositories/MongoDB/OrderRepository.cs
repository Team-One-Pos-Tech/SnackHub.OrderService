using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.Entities;

namespace SnackHub.OrderService.Infra.Repositories.MongoDB;

public sealed class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    private readonly ILogger<OrderRepository> _logger;
    
    public OrderRepository(
        ILogger<OrderRepository> logger,
        IMongoDatabase mongoDatabase, 
        string collectionName = "Orders") 
        : base(mongoDatabase, collectionName)
    {
        _logger = logger;
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
        var query = MongoCollection
            .AsQueryable()
            .OrderBy(o => o.CreatedAt);
        
        _logger.LogDebug("MongoDB query: {Query}", query);
        
        var result = query.ToList();
        
        return await Task.FromResult(result);
    }
}
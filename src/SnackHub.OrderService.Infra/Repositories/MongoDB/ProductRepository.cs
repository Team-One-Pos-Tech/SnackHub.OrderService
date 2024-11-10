using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.Entities;

namespace SnackHub.OrderService.Infra.Repositories.MongoDB;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;
    
    public ProductRepository(
        IMongoDatabase mongoDatabase, 
        ILogger<ProductRepository> logger, 
        string collectionName = "Products") 
        : base(mongoDatabase, collectionName)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await ListByPredicateAsync(p => ids.Contains(p.Id));
    }

    public async Task AddAsync(Product product)
    {
        await InsertAsync(product);
    }

    public async Task EditAsync(Product product)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
        await MongoCollection.ReplaceOneAsync(filter, product);
    }

    public async Task RemoveAsync(Guid id)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        await MongoCollection.DeleteOneAsync(filter);
    }
    
    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await FindByPredicateAsync(p => p.Id.Equals(id));
    }
}
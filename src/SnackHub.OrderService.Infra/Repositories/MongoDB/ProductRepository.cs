using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.Entities;

namespace SnackHub.OrderService.Infra.Repositories.MongoDB;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(
        IMongoDatabase mongoDatabase, 
        string collectionName = "Products") 
        : base(mongoDatabase, collectionName)
    {
    }

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await ListByPredicateAsync(p => ids.Contains(p.Id));
    }

    public async Task AddAsync(Product product)
    {
        await InsertAsync(product);
    }

    public async Task AddManyAsync(IEnumerable<Product> products)
    {
        await InsertList(products);
    }

    public async Task EditAsync(Product product)
    {
        await UpdateByPredicateAsync(px => px.Id == product.Id, product);
    }

    public async Task RemoveAsync(Guid id)
    {
        await DeleteByPredicateAsync(product => product.Id == id);
    }
    
    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await FindByPredicateAsync(p => p.Id.Equals(id));
    }
}
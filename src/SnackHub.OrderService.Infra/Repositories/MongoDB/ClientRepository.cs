using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.Entities;

namespace SnackHub.OrderService.Infra.Repositories.MongoDB;

public class ClientRepository : BaseRepository<Client>, IClientRepository
{
    public ClientRepository(
        IMongoDatabase mongoDatabase, 
        string collectionName = "Clients") 
        : base(mongoDatabase, collectionName)
    {
    }

    public async Task AddAsync(Client client)
    {
        await InsertAsync(client);
    }

    public async Task RemoveAsync(Guid identifier)
    {
        await DeleteByPredicateAsync(client => client.Id.Equals(identifier));
    }

    public async Task<Client?> GetByIdentifierAsync(Guid id)
    {
        return await FindByPredicateAsync(client => client.Id.Equals(id));
    }
}
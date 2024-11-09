using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.Entities;

namespace SnackHub.OrderService.Infra.Repositories.MongoDB;

public class ClientRepository : BaseRepository<Client>, IClientRepository
{
    private readonly ILogger<ClientRepository> _logger;
    
    public ClientRepository(
        IMongoDatabase mongoDatabase, 
        ILogger<ClientRepository> logger, 
        string collectionName = "Clients") 
        : base(mongoDatabase, collectionName)
    {
        _logger = logger;
    }

    public async Task AddAsync(Client client)
    {
        await InsertAsync(client);
    }

    public async Task RemoveAsync(Guid identifier)
    {
        await DeleteByPredicateAsync(client => client.Identifier.Equals(identifier));
    }

    public async Task<Client?> GetByIdentifierAsync(Guid id)
    {
        return await FindByPredicateAsync(x => x.Identifier.Equals(id));
    }
}
using System;
using System.Threading.Tasks;
using SnackHub.OrderService.Domain.Entities;

namespace SnackHub.OrderService.Domain.Contracts;

public interface IClientRepository
{
    Task AddAsync(Client client);
    Task RemoveAsync(Guid identifier);
    Task<Client?> GetByIdentifierAsync(Guid id);
}
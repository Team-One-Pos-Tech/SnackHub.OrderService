using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SnackHub.OrderService.Application.Models.Client;
using SnackHub.OrderService.Domain.Contracts;

namespace SnackHub.OrderService.Application.EventConsumers.Client;

public class ClientRemovedConsumer : IConsumer<ClientRemoved>
{
    private readonly ILogger<ClientRemovedConsumer> _logger;
    private readonly IClientRepository _clientRepository;

    public ClientRemovedConsumer(
        ILogger<ClientRemovedConsumer> logger, 
        IClientRepository clientRepository)
    {
        _logger = logger;
        _clientRepository = clientRepository;
    }

    public async Task Consume(ConsumeContext<ClientRemoved> context)
    {
        _logger.LogInformation("The client with identifier [{clientIdentifier}] has been removed service by a external service!", context.Message.Identifier);

        var client = new Domain.Entities.Client(context.Message.Identifier);
        await _clientRepository.AddAsync(client);
    }
}
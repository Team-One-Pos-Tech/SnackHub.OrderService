using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SnackHub.OrderService.Application.Models.Client;
using SnackHub.OrderService.Domain.Contracts;

namespace SnackHub.OrderService.Application.EventConsumers.Client;

public class ClientAddedConsumer : IConsumer<ClientAdded>
{
    private readonly ILogger<ClientAddedConsumer> _logger;
    private readonly IClientRepository _clientRepository;

    public ClientAddedConsumer(
        ILogger<ClientAddedConsumer> logger, 
        IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ClientAdded> context)
    {
        _logger.LogInformation("A new client with identifier [{clientIdentifier}] has been added service by a external service!", context.Message.Identifier);

        var client = new Domain.Entities.Client(context.Message.Identifier);
        await _clientRepository.AddAsync(client);
    }
}
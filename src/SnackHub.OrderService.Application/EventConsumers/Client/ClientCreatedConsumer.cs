using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SnackHub.OrderService.Application.Models.Client;
using SnackHub.OrderService.Domain.Contracts;

namespace SnackHub.OrderService.Application.EventConsumers.Client;

public class ClientCreatedConsumer : IConsumer<ClientCreated>
{
    private readonly ILogger<ClientCreatedConsumer> _logger;
    private readonly IClientRepository _clientRepository;

    public ClientCreatedConsumer(
        ILogger<ClientCreatedConsumer> logger, 
        IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ClientCreated> context)
    {
        _logger.LogInformation("A new client with identifier [{clientIdentifier}] has been added service by a external service!", context.Message.Id);

        var client = Domain.Entities.Client.Create(context.Message.Id, context.Message.Name, context.Message.Email);
        await _clientRepository.AddAsync(client);
    }
}
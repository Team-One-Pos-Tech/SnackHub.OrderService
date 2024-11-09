namespace SnackHub.OrderService.Api.Configuration;

public record StorageSettings
{
    public MongoDbSettings MongoDb { get; set; }
}
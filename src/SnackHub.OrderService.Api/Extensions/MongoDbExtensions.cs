using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using SnackHub.OrderService.Api.Configuration;

namespace SnackHub.OrderService.Api.Extensions;

public static class MongoDbExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var settings = configuration.GetSection("Storage:MongoDb").Get<MongoDbSettings>();
        var mongoClient = new MongoClient(settings.ConnectionString);

        serviceCollection.AddSingleton<IMongoClient>(_ => mongoClient);
        serviceCollection.AddSingleton<IMongoDatabase>(_ => mongoClient.GetDatabase(settings.Database));

        return serviceCollection;
    }
    
    // ReSharper disable UnusedMethodReturnValue.Global
    public static IApplicationBuilder UseMongoDbConventions(this IApplicationBuilder app)
    {
        var pack = new ConventionPack
        {
            new EnumRepresentationConvention(BsonType.String)
        };

        ConventionRegistry.Register("EnumStringConvention", pack, _ => true);
        
        return app;
    }
    // ReSharper restore UnusedMethodReturnValue.Global
}
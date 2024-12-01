using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using NUnit.Framework;

namespace SnackHub.OrderService.Behavior.Tests.Fixtures;

public abstract class MongoDbFixture
{
    private const int MongoDbPublicPort = 27017;
    private const string MongoDbDockerImage = "mongo";
    private const string MongoDbRootUserName = "admin";
    private const string MongoDbRootPassword = "Sup3rP4ss";

    private IContainer _mongoContainer;

    protected IMongoDatabase MongoDatabase;
    
    [SetUp]
    protected async Task BaseSetUp()
    {
        _mongoContainer = new ContainerBuilder()
            .WithImage(MongoDbDockerImage)
            .WithEnvironment("MONGO_INITDB_ROOT_USERNAME", MongoDbRootUserName)
            .WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", MongoDbRootPassword)
            .WithPortBinding(MongoDbPublicPort, true)
            .WithCleanUp(true)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilPortIsAvailable(MongoDbPublicPort))
            .Build();

        await _mongoContainer.StartAsync();

        var mongoConnectionString = $"mongodb://{MongoDbRootUserName}:{MongoDbRootPassword}@{_mongoContainer.Hostname}:{_mongoContainer.GetMappedPublicPort(MongoDbPublicPort)}";
        var mongoClient = new MongoClient(mongoConnectionString);
        
        BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        MongoDatabase = mongoClient.GetDatabase(Guid.NewGuid().ToString("D"));
    }

    [TearDown]
    protected async Task BaseTearDown()
    {
        await _mongoContainer.StopAsync();
        await _mongoContainer.DisposeAsync();
    }
}
using k8s.Models;

var builder = DistributedApplication.CreateBuilder(args);


// Backing servers
var postgres = builder
    .AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase("catalogdb");

var rabbitmq = builder
    .AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

// Projects
var catalog = builder.AddProject<Projects.Catalog>("catalog")
    .WithReference(catalogDb)
    .WithReference(rabbitmq)
    .WaitFor(catalogDb)
    .WaitFor(rabbitmq);

var cache = builder
    .AddRedis("cache")
    .WithRedisInsight()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);
// add projects and cloud-native backing services here
var basket = builder.AddProject<Projects.Basket>("basket")
    .WithReference(cache)
    .WithReference(catalog)
    .WithReference(rabbitmq)
    .WaitFor(cache)
    .WaitFor(rabbitmq);




builder.Build().Run();

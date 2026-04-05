using Basket.ApiClients;
using Basket.Endpoints;
using Basket.Services;
using ServiceDefaults.Messaging;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisDistributedCache(connectionName: "cache");
builder.Services.AddScoped<BasketService>();
builder.Services.AddMassTransitWithAssemblies(Assembly.GetExecutingAssembly());
// Add services to the container.

builder.Services.AddHttpClient<CatalogApiClient>(client =>
{
    client.BaseAddress = new("https+http://catalog");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();
app.MapBasketEndpoints();

app.UseHttpsRedirection();


app.Run();

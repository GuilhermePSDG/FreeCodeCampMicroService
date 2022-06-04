using Play.Catalog.Service.Models;
using Play.Catalog.Service.Settings;
using Play.Common.MongoDb;
using Play.Common.Settings;
using Play.Catalog.Contracts;
using MassTransit;
using MassTransit.Definition;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SubmitOrderConsumer>();
    x.UsingRabbitMq((ctx, conf) =>
    {
        var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        conf.Host(rabbitMqSettings.Host);
        conf.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
    });
});
builder.Services.AddMassTransitHostedService();
builder.Services.AddMongoDb();
builder.Services.AddScopedMongoDbRepository<Item>("items");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



class SubmitOrderConsumer : IConsumer<CatalogItemCreated>
{
    public async Task Consume(ConsumeContext<CatalogItemCreated> context)
    {
        Console.WriteLine("CONSUME EXECUTED");
    }
}
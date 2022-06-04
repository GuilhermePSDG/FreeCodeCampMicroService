using Play.Common.MassTransit;
using Play.Common.MongoDb;
using Play.Invetory.Service;
using Play.Invetory.Service.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddMongoDb()
    .AddScopedMongoDbRepository<InventoryItem>("inventoryitems")
    .AddScopedMongoDbRepository<CatalogItem>("catalogitems")
    .AddMassTransitWithRabbitMQ();

builder.AddCatalogClient();

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


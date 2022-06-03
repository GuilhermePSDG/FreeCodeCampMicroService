using MongoDB.Driver;
using Play.Catalog.Service.Models;
using Play.Catalog.Service.Repository;
using Play.Catalog.Service.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddScoped(x =>
{
    var mongoDbSettings = builder.Configuration.GetRequiredSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    var serviceSettings = builder.Configuration.GetRequiredSection(nameof(ServiceSettings)).Get<ServiceSettings>();
    return new MongoClient(mongoDbSettings.ConnectionString).GetDatabase(serviceSettings.ServiceName);
});

builder.Services.AddScoped<IRepository<Item>, MongoRepository<Item>> ();


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

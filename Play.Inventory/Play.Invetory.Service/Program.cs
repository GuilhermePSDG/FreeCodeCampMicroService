using Play.Common.MongoDb;
using Play.Invetory.Service.Clients;
using Play.Invetory.Service.Models;
using Polly;
using Polly.Timeout;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddMongoDb()
    .AddScopedMongoDbRepository<InventoryItem>("inventoryitems");

builder.Services
    .AddHttpClient<CatalogClient>(x => x.BaseAddress = new Uri("https://localhost:5000"))
    .AddTransientHttpErrorPolicy(b => b.Or<TimeoutRejectedException>().WaitAndRetryAsync(3,(attempt) => TimeSpan.FromSeconds(Math.Pow(2,attempt)) + TimeSpan.FromMilliseconds(Random.Shared.NextDouble() * 1000)))
    .AddTransientHttpErrorPolicy(b => b.Or<TimeoutRejectedException>()
    .CircuitBreakerAsync
        (
            handledEventsAllowedBeforeBreaking:10,
            durationOfBreak:TimeSpan.FromSeconds(15),
            onBreak:(res,timeSpan) => { },
            onReset:() => { }
        )
    )
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

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

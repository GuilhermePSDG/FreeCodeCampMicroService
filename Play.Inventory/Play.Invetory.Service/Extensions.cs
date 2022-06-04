using Play.Invetory.Service.Models;
using Play.Invetory.Service.Dtos;
using Play.Invetory.Service.Clients;
using Polly;
using Polly.Timeout;

namespace Play.Invetory.Service
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item,string Name,string Description) => new InventoryItemDto(item.CatalogItemId, Name,Description,item.Quantity, item.AcquiredDate);
        public static WebApplicationBuilder AddCatalogClient(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddHttpClient<CatalogClient>(x => x.BaseAddress = new Uri("https://localhost:5000"))
                .AddTransientHttpErrorPolicy
                    (b => b.Or<TimeoutRejectedException>()
                        .WaitAndRetryAsync(
                            3, 
                            (attempt) => TimeSpan.FromSeconds(Math.Pow(2, attempt)) + 
                                TimeSpan.FromMilliseconds(Random.Shared.NextDouble() * 1000)))


                .AddTransientHttpErrorPolicy(b => b.Or<TimeoutRejectedException>()
                .CircuitBreakerAsync
                    (
                        handledEventsAllowedBeforeBreaking: 10,
                        durationOfBreak: TimeSpan.FromSeconds(15),
                        onBreak: (res, timeSpan) => { },
                        onReset: () => { }
                    )
                )
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
            return builder;
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Play.Common.Models;
using Play.Common.Settings;
using Play.Common.Repository;

namespace Play.Common.MongoDb
{
    public static class Extensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection Services)
        {
            Services.AddScoped(x =>
            {
                var config = x.GetRequiredService<IConfiguration>();
                var mongoDbSettings = config.GetRequiredSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var serviceSettings = config.GetRequiredSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                return new MongoClient(mongoDbSettings.ConnectionString).GetDatabase(serviceSettings.ServiceName);
            });
            return Services;
        }
        public static IServiceCollection AddScopedMongoDbRepository<T>(this IServiceCollection services, string CollectionName) where T : IEntity
        {
            services.AddScoped<IRepository<T>>((pro) => new MongoRepository<T>(pro.GetRequiredService<IMongoDatabase>(), CollectionName));
            return services;

        }
    }
}

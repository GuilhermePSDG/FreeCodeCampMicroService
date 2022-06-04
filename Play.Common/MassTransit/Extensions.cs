using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;
using System.Reflection;

namespace Play.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services,params Assembly[] IncludeAssemblys)
        {
            services.AddMassTransit(x =>
            {
                var assemblys = IncludeAssemblys.ToList();
                assemblys.Add(Assembly.GetEntryAssembly() ?? throw new NullReferenceException(nameof(Assembly.GetEntryAssembly)));
                x.AddConsumers(assemblys.ToArray());
                x.UsingRabbitMq((ctx, conf) =>
                {
                    var Configuration = ctx.GetRequiredService<IConfiguration>();
                    var serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var rabbitMqSettings =Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    conf.Host(rabbitMqSettings.Host);
                    conf.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                });
            });
            services.AddMassTransitHostedService();
            return services;
        }
    }
}

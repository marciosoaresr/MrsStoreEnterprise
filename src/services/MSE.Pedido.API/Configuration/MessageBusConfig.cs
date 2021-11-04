using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSE.Core.Utils;
using MSE.MessageBus;
using MSE.Pedidos.API.Services;

namespace MSE.Pedidos.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<PedidoOrquestradorIntegrationHandler>()
                .AddHostedService<PedidoIntegrationHandler>();
        }
    }
}
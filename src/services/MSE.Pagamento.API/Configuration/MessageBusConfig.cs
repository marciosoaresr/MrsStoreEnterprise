using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSE.Core.Utils;
using MSE.MessageBus;
using MSE.Pagamentos.API.Services;

namespace MSE.Pagamentos.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<PagamentoIntegrationHandler>();
        }
    }
}
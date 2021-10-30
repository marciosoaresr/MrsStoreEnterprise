using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSE.Carrinho.API.Data;
using MSE.MessageBus;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MSE.Carrinho.API.Services
{
    public class CarrinhoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProdiver;

        public CarrinhoIntegrationHandler(IMessageBus bus, IServiceProvider serviceProdiver)
        {
            _bus = bus;
            _serviceProdiver = serviceProdiver;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();

            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<PedidoRealizadoIntegrationEvent>("PedidoRealizado", async request => await ApagarCarrinho(request));
        }

        private async Task ApagarCarrinho(PedidoRealizadoIntegrationEvent message)
        {
            using var scope = _serviceProdiver.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<CarrinhoContext>();

            var carrinho = await context.CarrinhoCliente
               .FirstOrDefaultAsync(c => c.ClienteId == message.ClienteId);

            if (carrinho != null)
            {
                context.CarrinhoCliente.Remove(carrinho);
                await context.SaveChangesAsync();
            }
        }
    }
}

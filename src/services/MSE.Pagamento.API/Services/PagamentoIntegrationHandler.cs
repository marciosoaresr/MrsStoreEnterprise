﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSE.Core.DomainObjects;
using MSE.Core.Messages.Integration;
using MSE.MessageBus;
using MSE.Pagamentos.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MSE.Pagamentos.API.Services
{
    public class PagamentoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public PagamentoIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            setResponder();

            setSubscribers();

            return Task.CompletedTask;
        }

        private void setResponder()
        {
            _bus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request => await AutorizarPagamento(request));
        }

        private void setSubscribers()
        {
            _bus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado", async request => await CancelarPagamento(request));

            _bus.SubscribeAsync<PedidoBaixadoEstoqueIntegrationEvent>("PedidoBaixadoEstoque", async request => await CapturarPagamento(request));
        }

        private async Task<ResponseMessage> AutorizarPagamento(PedidoIniciadoIntegrationEvent message)
        {
            ResponseMessage response;

            using(var scope = _serviceProvider.CreateScope())
            {
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

                var pagamento = new Pagamento
                {
                    PedidoId = message.PedidoId,
                    TipoPagamento = (TipoPagamento)message.TipoPagamento,
                    Valor = message.Valor,
                    CartaoCredito = new CartaoCredito(message.NomeCartao, message.NumeroCartao, message.MesAnoVencimento, message.CVV)
                };

                response = await pagamentoService.AutorizarPagamento(pagamento);
            }

            return response;
        }
        private async Task<ResponseMessage> CapturarPagamento(PedidoBaixadoEstoqueIntegrationEvent message)
        {
            using(var scope = _serviceProvider.CreateScope())
            {
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

                var response = await pagamentoService.CapturarPagamento(message.PedidoId);

                if (!response.ValidationResult.IsValid)
                {
                    throw new DomainException($"Falha ao capturar pagamento do pedido {message.PedidoId}");
                }

                await _bus.PublishAsync(new PedidoPagoIntegrationEvent(message.ClienteId, message.PedidoId));

                return response;
            }

        }

        private async Task<ResponseMessage> CancelarPagamento(PedidoCanceladoIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

                var response = await pagamentoService.CancelarPagamento(message.PedidoId);

                if (!response.ValidationResult.IsValid)
                {
                    throw new DomainException($"Falha ao cancelar pagamento do pedido {message.PedidoId}");
                }

                return response;
            }
        }
    }
}

using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MSE.Clientes.API.Application.Commands;
using MSE.Clientes.API.Application.Events;
using MSE.Clientes.API.Data.Repository;
using MSE.Clientes.API.Models;
using MSE.Core.Mediator;
using NSE.Clientes.API.Data;

namespace MSE.Clientes.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();

            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ClientesContext>();

        }
    }
}
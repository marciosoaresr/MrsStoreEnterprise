﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MSE.Bff.Compras.Extensions;
using MSE.Bff.Compras.Models;

namespace MSE.Bff.Compras.Services
{
    public interface IClienteService
    {
        Task<EnderecoDTO> ObterEndereco();
    }

    public class ClienteService : Service, IClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
        }

        public async Task<EnderecoDTO> ObterEndereco()
        {
            var response = await _httpClient.GetAsync("/cliente/endereco/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<EnderecoDTO>(response);
        }
    }
}
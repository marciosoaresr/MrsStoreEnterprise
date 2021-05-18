using Microsoft.Extensions.DependencyInjection;
using MSE.WebApp.MVC.Services;

namespace MSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();
        }
    }
}

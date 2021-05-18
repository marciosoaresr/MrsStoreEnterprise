using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSE.WebApp.MVC.Extensions;
using MSE.WebApp.MVC.Services;

namespace MSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUser, AspNetUser>();
        }
    }
}

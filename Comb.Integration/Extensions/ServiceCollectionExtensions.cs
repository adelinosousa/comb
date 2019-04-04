using Microsoft.Extensions.DependencyInjection;

namespace Comb.Integration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddComb(this IServiceCollection services)
        {
            services.AddSingleton<ICombHttpClient, CombHttpClient>();
            return services;
        }
    }
}

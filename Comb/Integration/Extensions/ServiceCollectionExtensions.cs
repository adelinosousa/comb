using Microsoft.Extensions.DependencyInjection;

namespace Site.Comb
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSiteComb(this IServiceCollection services)
        {
            services.AddSingleton<ICombHttpClient, CombHttpClient>();
            services.AddTransient<IComb, Comb>();
            return services;
        }
    }
}

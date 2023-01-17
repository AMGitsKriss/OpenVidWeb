using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserManager
{
    public static class Installer
    {
        public static IServiceCollection CatalogManagerInstaller(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}

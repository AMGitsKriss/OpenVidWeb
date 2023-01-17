using CatalogManager.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogManager
{
    public static class Installer
    {
        public static IServiceCollection CatalogManagerInstaller(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CatalogImportOptions>(configuration.GetSection("Catalog"));
            services.Configure<PermissionOptions>(configuration.GetSection("Authentication"));

            services.AddScoped<ImportService>();
            services.AddScoped<ThumbnailService>();
            services.AddScoped<PermissionsService>();
            services.AddScoped<IMetadataStrategy, FFMpegStrategy>();

            return services;
        }
    }
}

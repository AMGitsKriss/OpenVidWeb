using Microsoft.Extensions.DependencyInjection;

namespace TagCache
{
    public static class Installer
    {
        public static void TagCacheInstaller(this IServiceCollection services)
        {
            services.AddScoped<TagManager>();

            services.AddScoped<RelatedTags>();
            services.AddScoped<SuggestedTags>();
        }
    }
}

using System;
using System.Linq;
using EPiServer.Shell.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Advanced.CMS.MediaDownload
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediaDownload(this IServiceCollection services)
        {
            services.Configure<ProtectedModuleOptions>(
                pm =>
                {
                    if (!pm.Items.Any(i =>
                        i.Name.Equals("advanced-cms-media-download", StringComparison.OrdinalIgnoreCase)))
                    {
                        pm.Items.Add(new ModuleDetails { Name = "advanced-cms-media-download" });
                    }
                });

            return services;
        }
    }
}

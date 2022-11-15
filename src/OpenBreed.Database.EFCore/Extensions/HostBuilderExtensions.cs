using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.EFCore.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupEFDatabaseContext(this IHostBuilder hostBuilder, Action<OpenBreedDbContext, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<OpenBreedDbContext>((sp) =>
                {
                    var context = new OpenBreedDbContext();
                    action.Invoke(context, sp);
                    return context;
                });
            });
        }
    }
}

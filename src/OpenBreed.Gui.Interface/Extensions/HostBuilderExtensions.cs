using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void ConfigureInteraction(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IInteractionCore, InteractionCore>();
            });
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Gui.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void ConfigureInteraction(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IInteractionCore, InteractionCore>();
                services.AddSingleton<IInteractionRenderer, InteractionRenderer>();
                services.AddSingleton<IElementRenderer, ButtonRenderer>();
                services.AddSingleton<IElementRenderer, LabelRenderer>();
                services.AddSingleton<IElementRenderer, PanelRenderer>();
                services.AddSingleton<IElementRenderer, GridPanelRenderer>();
                services.AddSingleton<IElementRenderer, StateboxRenderer>();
            });
        }
    }
}

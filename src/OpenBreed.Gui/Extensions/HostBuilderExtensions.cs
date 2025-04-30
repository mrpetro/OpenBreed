using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Gui.Builders;
using OpenBreed.Gui.Controllers;
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



                services.AddSingleton<IInteractionFactoryProvider, InteractionFactoryProvider>();
                services.AddTransient<IInteractionFactory, InteractionFactory>();
                services.AddSingleton<IInteractionRenderer, InteractionRenderer>();
                services.AddSingleton<IElementRenderer, DesktopRenderer>();
                services.AddSingleton<IElementRenderer, ButtonRenderer>();
                services.AddSingleton<IElementRenderer, LabelRenderer>();
                services.AddSingleton<IElementRenderer, PanelRenderer>();
                services.AddSingleton<IElementRenderer, GridPanelRenderer>();
                services.AddSingleton<IElementRenderer, StateboxRenderer>();
                services.AddSingleton<IElementRenderer, TextFieldRenderer>();
                services.AddSingleton<IElementRenderer, ScrollbarRenderer>();
                services.AddSingleton<ICursorRenderer, CursorRenderer>();


                services.AddTransient<DesktopBuilder>();
                services.AddTransient<GridPanelBuilder>();
                services.AddTransient<DockPanelBuilder>();
                services.AddTransient<ButtonBuilder>();
                services.AddTransient<LabelFieldBuilder>();
                services.AddTransient<TextFieldBuilder>();
                services.AddTransient<CheckboxBuilder>();

                services.AddSingleton<IElementInputController<IButton>, ButtonInputController>();
                services.AddSingleton<IElementInputController<IGridPanel>, GridPanelInputController>();
                services.AddSingleton<IElementInputController<ITextField>, TextFieldInputController>();
                services.AddSingleton<IElementInputController<IDesktop>, DesktopInputController>();
                services.AddSingleton<IElementInputController<ICheckField>, CheckFieldInputController>();
                services.AddSingleton<IElementInputController<IScrollbar>, ScrollbarInputController>();
                services.AddSingleton<IElementInputController, DefaultInputController>();

                services.AddSingleton<IElementFactory, ElementFactory>();
            });
        }
    }
}

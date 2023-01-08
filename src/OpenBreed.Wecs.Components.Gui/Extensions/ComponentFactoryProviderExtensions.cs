using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Wecs.Components.Gui.Xml;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Gui.Extensions
{
    public static class ComponentFactoryProviderExtensions
    {
        public static void SetupXmlGuiComponents(this IComponentFactoryProvider provider, IServiceProvider serviceProvider)
        {
            provider.RegisterComponentFactory<XmlCursorInputComponent>(serviceProvider.GetService<CursorInputComponentFactory>());
        }
    }
}

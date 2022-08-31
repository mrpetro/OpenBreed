using Microsoft.Extensions.Hosting;
using OpenBreed.Wecs.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Components.Scripting.Xml;

namespace OpenBreed.Wecs.Components.Scripting.Extensions
{
    public static class ComponentFactoryProviderExtensions
    {
        public static void SetupScriptingComponents(this IComponentFactoryProvider provider, IServiceProvider serviceProvider)
        {
            provider.RegisterComponentFactory<XmlScriptRunnerComponent>(serviceProvider.GetService<ScriptRunnerComponentFactory>());
        }
    }
}

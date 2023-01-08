using Microsoft.Extensions.Hosting;
using OpenBreed.Wecs.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Fsm.Xml;

namespace OpenBreed.Fsm.Extensions
{
    public static class ComponentFactoryProviderExtensions
    {
        public static void SetupXmlFsmComponents(this IComponentFactoryProvider provider, IServiceProvider serviceProvider)
        {
            provider.RegisterComponentFactory<XmlFsmComponent>(serviceProvider.GetService<FsmComponentFactory>());
        }
    }
}

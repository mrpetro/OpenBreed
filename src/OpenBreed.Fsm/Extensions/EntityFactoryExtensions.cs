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
    public static class EntityFactoryExtensions
    {
        public static void SetupFsmComponents(this IEntityFactory entityFactory, IServiceProvider serviceProvider)
        {
            entityFactory.RegisterComponentFactory<XmlFsmComponent>(serviceProvider.GetService<FsmComponentFactory>());
        }
    }
}

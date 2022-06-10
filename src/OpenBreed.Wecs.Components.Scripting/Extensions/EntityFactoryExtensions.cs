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
    public static class EntityFactoryExtensions
    {
        public static void SetupScriptingComponents(this IEntityFactory entityFactory, IServiceProvider serviceProvider)
        {
            entityFactory.RegisterComponentFactory<XmlScriptRunnerComponent>(serviceProvider.GetService<ScriptRunnerComponentFactory>());
        }
    }
}

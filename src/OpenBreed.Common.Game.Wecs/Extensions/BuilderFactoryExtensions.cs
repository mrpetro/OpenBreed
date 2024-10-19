using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Common.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Core.Managers;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Core.Interface.Managers;

namespace OpenBreed.Common.Game.Wecs.Extensions
{
    public static class BuilderFactoryExtensions
    {
        public static void SetupSandboxBuilders(this IBuilderFactory builderFactory, IServiceProvider sp)
        {
            var dataLoderFactory = sp.GetService<IDataLoaderFactory>();

            builderFactory.Register<DataGridComponentBuilder>(
                () => new DataGridComponentBuilder(
                    sp.GetService<IDataGridFactory>()));
        }
    }
}

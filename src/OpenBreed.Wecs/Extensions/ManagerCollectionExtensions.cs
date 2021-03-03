using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupWecsManagers(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<IEntityMan>(() => new EntityMan(manCollection.GetManager<ICore>(),
                                                                       manCollection.GetManager<ICommandsMan>()));

            manCollection.AddSingleton<ISystemFactory>(() => new DefaultSystemFactory());

            manCollection.AddSingleton<IEntityFactory>(() => new EntityFactory(manCollection.GetManager<IEntityMan>()));

            manCollection.AddSingleton<IWorldMan>(() => new WorldMan(manCollection.GetManager<IEntityMan>(),
                                                                     manCollection.GetManager<ICommandsMan>(),
                                                                     manCollection.GetManager<IEventsMan>(),
                                                                     manCollection.GetManager<IScriptMan>(),
                                                                     manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<ISystemFinder>(() => new SystemFinder(manCollection.GetManager<IEntityMan>(),
                                                                             manCollection.GetManager<IWorldMan>()));
        }
    }
}

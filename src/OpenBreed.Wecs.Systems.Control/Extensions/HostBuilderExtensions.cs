using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Systems.Core;

namespace OpenBreed.Wecs.Systems.Control.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupControlSystems(this ISystemFactory systemFactory, IServiceProvider sp)
        {
            systemFactory.RegisterSystem<ActionControlSystem>((world) => new ActionControlSystem(
                world,
                sp.GetRequiredService<IEntityMan>(),
                sp.GetRequiredService<IInputsMan>(),
            sp.GetRequiredService<IEventsMan>()));
            systemFactory.RegisterSystem<FollowerSystem>((world) => new FollowerSystem(
                world,
                sp.GetRequiredService<IWorldMan>(),
                sp.GetRequiredService<IEntityMan>(),
                sp.GetRequiredService<IEventsMan>()));
        }
    }
}

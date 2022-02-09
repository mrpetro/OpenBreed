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

namespace OpenBreed.Wecs.Systems.Control.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupControlSystems(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.Register<AiControlSystem>(() => new AiControlSystem());
            systemFactory.Register<WalkingControllerSystem>(() => new WalkingControllerSystem(serviceProvider.GetService<IPlayersMan>()));
            systemFactory.Register<AttackControllerSystem>(() => new AttackControllerSystem(serviceProvider.GetService<IPlayersMan>()));
            systemFactory.Register<FollowerSystem>(() => new FollowerSystem(serviceProvider.GetService<IEntityMan>()));
        }
    }
}

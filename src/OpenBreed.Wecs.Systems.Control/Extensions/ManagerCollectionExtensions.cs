using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Control.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupControlSystems(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new AiControlSystem());
            systemFactory.Register(() => new WalkingControlSystem(manCollection.GetManager<IEntityMan>()));
            systemFactory.Register(() => new WalkingControllerSystem(manCollection.GetManager<IPlayersMan>()));
            systemFactory.Register(() => new AttackControllerSystem(manCollection.GetManager<IPlayersMan>()));
            systemFactory.Register(() => new FollowerSystem(manCollection.GetManager<IEntityMan>()));
        }
    }
}

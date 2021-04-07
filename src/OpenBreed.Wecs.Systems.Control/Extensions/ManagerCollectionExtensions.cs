using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Commands;
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
            systemFactory.Register(() => new WalkingControlSystem());
            systemFactory.Register(() => new WalkingControllerSystem(manCollection.GetManager<IPlayersMan>()));
            systemFactory.Register(() => new AttackControllerSystem(manCollection.GetManager<IPlayersMan>(),
                                                                    manCollection.GetManager<ICommandsMan>()));
            systemFactory.Register(() => new FollowerSystem(manCollection.GetManager<IEntityMan>(),
                                                            manCollection.GetManager<ICommandsMan>()));

            var entityCommandHandler = manCollection.GetManager<EntityCommandHandler>();

            entityCommandHandler.BindCommand<FollowedAddFollowerCommand, FollowerSystem>();
        }
    }
}

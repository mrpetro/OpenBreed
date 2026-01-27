using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Animation.Systems.Extensions;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Physics.Systems.Abstractions;
using OpenBreed.Wecs.Physics.Systems.Helpers;
using OpenBreed.Wecs.Scripting.Systems.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using OpenBreed.Common.Game.Wecs.Systems.Projectile;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class OnActorTouchExitTriggerSystem : IOnActorTouchObstacleSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public OnActorTouchExitTriggerSystem(
            IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "ActorTouch";
        public string ActionName => "Exit";

        #endregion Public Properties

        #region Public Methods

        public void OnTouch(
            IFixture actorFixture, IEntity actorEntity,
            IFixture triggerFixture, IEntity triggerEntity,
            Vector2 projection)
        {
            // For preventing running rest of the code when actor will hit couple of teleporter blocks at same time
            if (Equals(actorEntity.State, "Exiting"))
            {
                return;
            }

            var exitEntity = triggerEntity;

            actorEntity.State = "Exiting";

            var cameraEntity = actorEntity.TryGet<FollowedComponent>()?.FollowerIds.
                                                                              Select(item => services.Entities.GetById(item)).
                                                                              FirstOrDefault(item => item.Tag is "Camera.Player");

            if (cameraEntity is null)
            {
                return;
            }

            var matadataCmp = exitEntity.Get<MetadataComponent>();

            if (!int.TryParse(matadataCmp.Flavor, out int exitId))
            {
                throw new InvalidOperationException("Expected exit number");
            }

            var mapId = exitId % 64;
            var entryId = exitId / 64;

            var mapKey = $"Vanilla/{mapId}";

            var worldIdToRemoveFrom = actorEntity.WorldId;

            var actorWorld = services.Worlds.GetById(actorEntity.WorldId);
            var cameraWorld = services.Worlds.GetById(cameraEntity.WorldId);
            //var doorOpening = PerformFunction(() => door.TryOpen(key));
            //var doorClosing = doorOpening.OnFinishResult((result) => result == "Matching").PerformAction(() => door.Close())
            //door.Wait(5).OnFinish((door) => door.Close())
            //door.Close()

            var task = Core.Task.Create((t) => services.PauseWorld(t, actorEntity));

            task.Then((job) => services.FadeOut(job, cameraEntity))
                .Then((job) => services.RemoveFromWorld(job, actorEntity))
                .Then((job) => services.LoadWorld(job, mapKey))
                .Then((job) => services.AddToWorld(job, actorEntity, mapKey))
                .Then((job) => services.PlayerCharacterEnter(job, actorEntity, entryId));

            task.Start();
        }

        #endregion Public Methods
    }
}
using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Abstractions;
using OpenBreed.Animation.Generic;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Projectile;
using OpenBreed.Common.Interface;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Abstractions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Physics.Systems.Abstractions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class TeleportingSystem : IOnEntityCollisionSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public TeleportingSystem(
            IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public int ColliderTypeA => ColliderTypes.ActorBody;

        public IEnumerable<int> ColliderTypesB
        {
            get
            {
                yield return ColliderTypes.TeleportTrigger;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void OnCollision(IFixture actorFixture, IEntity actorEntity,
            IFixture triggerFixture, IEntity teleportEntity, float dt,
            Vector2 projection)
        {
            var cameraEntity = services.Entities.GetPlayerCamera(actorEntity);
            var hudCameraEntity = services.Entities.GetHudCamera();
            var cameraFadeInClipId = services.Clips.GetId("Vanilla/Common/Camera/Effects/FadeIn");
            var exitPosition = services.GetExitPosition(teleportEntity);

            if (Equals(actorEntity.State, "Teleporting"))
            {
                return;
            }

            actorEntity.State = "Teleporting";

            var task = Core.Task.Create((t) => services.PauseWorld(t, actorEntity));

            task.Then((t) => services.FadeOut(t, cameraEntity))
                .Then((t) => services.SetPosition(t, actorEntity, exitPosition))
                .Then((t) => services.UnpauseWorld(t, cameraEntity))
                .Then((t) => FadeIn(t));

            task.Start();

            void FadeIn(ITask task)
            {
                cameraEntity.PlayAnimation(0, cameraFadeInClipId);
                hudCameraEntity.PlayAnimation(0, cameraFadeInClipId);

                task.Finish();
            }
        }

        #endregion Public Methods
    }
}
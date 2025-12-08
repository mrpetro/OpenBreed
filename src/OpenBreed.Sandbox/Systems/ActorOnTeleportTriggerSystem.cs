using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;

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
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Physics.Abstractions;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Compute.OpenCL;
using OpenTK.Mathematics;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace OpenBreed.Sandbox.Systems
{
    public class ActorOnTeleportTriggerSystem : IEntityOnTriggerActionSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public ActorOnTeleportTriggerSystem(
            IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "ActorTouch";
        public string ActionName => "Teleport";

        #endregion Public Properties

        #region Public Methods

        public void OnTrigger(IEntity actorEntity, IEntity triggerEntity)
        {
            var teleportEntity = triggerEntity;
            var cameraEntity = services.Entities.GetPlayerCamera(actorEntity);
            var hudCameraEntity = services.Entities.GetHudCamera();
            var cameraFadeInClipId = services.Clips.GetId("Vanilla/Common/Camera/Effects/FadeIn");

            if (Equals(actorEntity.State, "Teleporting"))
            {
                return;
            }

            actorEntity.State = "Teleporting";

            var task = Core.Task.Create((t) => services.PauseWorld(t, actorEntity));

            task.Then((t) => services.FadeOut(t, cameraEntity))
                .Then((t) => SetPosition(t))
                .Then((t) => services.UnpauseWorld(t, cameraEntity))
                .Then((t) => FadeIn(t));

            task.Start();

            void SetPosition(ITask task)
            {
                services.Logger.LogInformation("SetPosition to exit...");

                actorEntity.SetPositionToExit(services.Entities, services.Shapes, teleportEntity);

                task.Finish();
            }

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
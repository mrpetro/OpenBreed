using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Sandbox.Components;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Wecs.Systems.Physics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Events;
using OpenBreed.Input.Interface;
using OpenBreed.Fsm;
using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Gui;
using OpenBreed.Wecs.Components.Gui;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Common;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Extensions;

namespace OpenBreed.Sandbox.Worlds
{
    public class GameWorldHelper
    {
        private readonly IPlayersMan playersMan;
        private readonly IEntityMan entityMan;
        private readonly ISystemFactory systemFactory;
        private readonly IWorldMan worldMan;
        private readonly ILogger logger;

        public GameWorldHelper(IPlayersMan playersMan, IEntityMan entityMan, ISystemFactory systemFactory, IWorldMan worldMan, ILogger logger)
        {
            this.playersMan = playersMan;
            this.entityMan = entityMan;
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            this.logger = logger;
        }

        public void AddSystems(WorldBuilder builder)
        {
            builder.SetupGameWorldSystems(systemFactory);
        }

        public void SetPreserveAspectRatio(Entity viewportEntity)
        {
            var cameraEntity = entityMan.GetById(viewportEntity.Get<ViewportComponent>().CameraEntityId);
            viewportEntity.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateCameraFov(cameraEntity, a));
        }

        private void UpdateCameraFov(Entity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.Get<CameraComponent>().Width = a.Width;
            cameraEntity.Get<CameraComponent>().Height = a.Height;
        }

        private void OnEntityAdded(object sender, EntityAddedEventArgs a)
        {
            var world = worldMan.GetById(a.WorldId);
            logger.Verbose($"Entity '{a.EntityId}' added to world '{world.Name}'.");
        }

        private void OnEntityRemoved(object sender, EntityRemovedEventArgs a)
        {
            var world = worldMan.GetById(a.WorldId);
            logger.Verbose($"Entity '{a.EntityId}' removed from world '{world.Name}'.");
        }
    }
}

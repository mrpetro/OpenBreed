using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Rendering.Systems.Events;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Common.Game.Wecs.Systems.Hud
{
    [RequireEntityWithTag("FpsCounter")]
    public class DebugHudInitSystem :
        IEventSystem<WorldInitializedEventArgs>,
        IEventSystem<ViewportResizedEvent>
    {
        private readonly IGameServices services;
        private readonly IWindow viewClient;

        public DebugHudInitSystem(IGameServices services,
            IWindow viewClient)
        {
            this.services = services;
            this.viewClient = viewClient;
        }

        public void OnEvent(WorldInitializedEventArgs e)
        {
            var world = services.Worlds.GetById(e.WorldId);

            if (world.Name != WorldNames.DebugHud)
            {
                return;
            }

            var hudCamera = services.Factory.CreateCamera(
                "Camera.DebugHud",
                0.0f,
                0.0f,
                viewClient.ClientRectangle.Size.X,
                viewClient.ClientRectangle.Size.Y);

            services.Worlds.RequestAddEntity(hudCamera, world.Id);

            AddFpsCounter(world);
            AddPositionInfo(world);

            var hudViewport = services.Entities.GetByTag(EntityNames.DebugHudViewport).First();
            hudViewport.SetViewportCamera(hudCamera.Id);

        }

        public void AddFpsCounter(IWorld world)
        {
            var fpsCounter = services.Factory.Create(@"ABTA\Templates\Common\Hud\FpsCounter")
                .SetParameter("posX", -viewClient.ClientRectangle.Size.X / 2.0f)
                .SetParameter("posY", -viewClient.ClientRectangle.Size.Y / 2.0f)
                .SetTag("FpsCounter")
                .Build();

            services.Worlds.RequestAddEntity(fpsCounter, world.Id);
        }

        public void AddPositionInfo(IWorld world)
        {
            var positionInfo = services.Factory.Create(@"ABTA\Templates\Common\Hud\PositionInfo")
                .SetParameter("posX", viewClient.ClientRectangle.Size.X / 2.0f - 180.0f)
                .SetParameter("posY", -viewClient.ClientRectangle.Size.Y / 2.0f)
                .SetTag("PositionInfo")
                .Build();

            services.Worlds.RequestAddEntity(positionInfo, world.Id);

            //var hudViewport = entityMan.GetByTag(ScreenWorldHelper.DEBUG_HUD_VIEWPORT).First();

            //jobsMan.Execute(new JohnPositionTextUpdateJob(entityMan, positionInfo));
        }


        public void OnEvent(ViewportResizedEvent e)
        {
            var viewportEntity = services.Entities.GetById(e.EntityId);

            if (viewportEntity.Tag != EntityNames.DebugHudViewport)
            {
                return;
            }

            var cameraEntity = services.Entities.GetByTag("Camera.DebugHud").FirstOrDefault();
            cameraEntity.Get<CameraComponent>().Size = new Vector2(e.Width, e.Height);

            var fpsTextEntity = services.Entities.GetByTag("PositionInfo").FirstOrDefault();
            fpsTextEntity?.SetPosition(e.Width / 2.0f - 180.0f, -e.Height / 2.0f);

            var fpsCounter = services.Entities.GetFpsCounter();
            fpsCounter?.SetPosition(-e.Width / 2.0f, -e.Height / 2.0f);
        }

    }
}

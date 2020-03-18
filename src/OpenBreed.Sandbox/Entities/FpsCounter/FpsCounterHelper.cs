using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Events;
using OpenBreed.Sandbox.Jobs;
using OpenBreed.Sandbox.Worlds;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.FpsCounter
{
    public static class FpsCounterHelper
    {
        public static void AddToWorld(World world)
        {
            var arial12 = world.Core.Rendering.Fonts.Create("ARIAL", 9);

            var fpsTextEntity = world.Core.Entities.Create();

            fpsTextEntity.Add(PositionComponent.Create(new Vector2(-fpsTextEntity.Core.ClientRectangle.Width / 2.0f, -fpsTextEntity.Core.ClientRectangle.Height / 2.0f)));
            fpsTextEntity.Add(TextComponent.Create(arial12.Id, Vector2.Zero, Color4.White, "FPS: 0.0", 100.0f));
            world.AddEntity(fpsTextEntity);


            var hudViewport = world.Core.Entities.GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();

            world.Core.Jobs.Execute(new FpsTextUpdateJob(fpsTextEntity));
            hudViewport.Subscribe(GfxEventTypes.VIEWPORT_RESIZED, (s, a) => UpdateFpsPos(fpsTextEntity, (ViewportResizedEventArgs)a));
        }

        private static void UpdateFpsPos(IEntity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.GetComponent<PositionComponent>().Value = new Vector2(-a.Width / 2.0f, -a.Height / 2.0f);
        }
    }
}

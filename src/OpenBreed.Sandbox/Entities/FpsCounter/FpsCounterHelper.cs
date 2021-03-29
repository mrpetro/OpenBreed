using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Sandbox.Jobs;
using OpenBreed.Sandbox.Worlds;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Commands;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Sandbox.Entities.FpsCounter
{
    public static class FpsCounterHelper
    {
        public static void AddToWorld(ICore core, World world)
        {
            var windowClient = core.GetManager<IViewClient>();
            var arial12 = core.GetManager<IFontMan>().Create("ARIAL", 10);

            var fpsTextEntity = core.GetManager<IEntityMan>().Create(core);

            fpsTextEntity.Add(PositionComponent.Create(new Vector2(-windowClient.ClientRectangle.Width / 2.0f, -windowClient.ClientRectangle.Height / 2.0f)));

            var textBuilder = core.GetManager<TextComponentBuilder>();
            textBuilder.SetFontById(arial12.Id);
            textBuilder.SetOffset(Vector2.Zero);
            textBuilder.SetColor(Color4.White);
            textBuilder.SetText("FPS: 0.0");
            textBuilder.SetOrder(100);



            fpsTextEntity.Add(textBuilder.Build());
            core.Commands.Post(new AddEntityCommand(world.Id, fpsTextEntity.Id));
            //world.AddEntity(fpsTextEntity);


            var hudViewport = core.GetManager<IEntityMan>().GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();

            core.Jobs.Execute(new FpsTextUpdateJob(core, fpsTextEntity));
            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateFpsPos(fpsTextEntity, a));
        }

        private static void UpdateFpsPos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(-a.Width / 2.0f, -a.Height / 2.0f);
        }
    }
}

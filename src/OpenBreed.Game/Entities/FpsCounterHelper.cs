using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities
{
    public static class FpsCounterHelper
    {
        public static void AddToWorld(World world)
        {
            var arial12 = world.Core.Rendering.Fonts.Create("ARIAL", 10);

            var fpsTextEntity = world.Core.Entities.Create();

            fpsTextEntity.Add(PositionComponent.Create(new Vector2(0,0)));

            var textBuilder = TextComponentBuilder.New(world.Core);
            textBuilder.SetProperty("FontId", arial12.Id);
            textBuilder.SetProperty("Offset", Vector2.Zero);
            textBuilder.SetProperty("Color", Color4.White);
            textBuilder.SetProperty("Text", "FPS: 0.0");
            textBuilder.SetProperty("Order", 100.0f);



            fpsTextEntity.Add(textBuilder.Build());
            world.Core.Commands.Post(new AddEntityCommand(world.Id, fpsTextEntity.Id));
            //world.AddEntity(fpsTextEntity);


            var gameViewport = world.Core.Entities.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();

            //world.Core.Jobs.Execute(new FpsTextUpdateJob(fpsTextEntity));
            gameViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateFpsPos(fpsTextEntity, a));
        }

        private static void UpdateFpsPos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(0.0f, 0.0f);
        }
    }
}

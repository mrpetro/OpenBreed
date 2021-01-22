using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Components.Common;
using OpenBreed.Components.Rendering;
using OpenBreed.Rendering.Interface;
using OpenBreed.Systems.Rendering.Events;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Ecsw;
using OpenBreed.Ecsw.Entities;

namespace OpenBreed.Game.Entities
{
    public static class FpsCounterHelper
    {
        public static void AddToWorld(World world)
        {
            var arial12 = world.Core.GetModule<IRenderModule>().Fonts.Create("ARIAL", 10);

            var fpsTextEntity = world.Core.GetManager<IEntityMan>().Create();

            fpsTextEntity.Add(PositionComponent.Create(new Vector2(0,0)));

            var textBuilder = TextComponentBuilderEx.New(world.Core);
            textBuilder.SetFontById(arial12.Id);
            textBuilder.SetOffset(Vector2.Zero);
            textBuilder.SetColor(Color4.White);
            textBuilder.SetText("FPS: 0.0");
            textBuilder.SetOrder(100);



            fpsTextEntity.Add(textBuilder.Build());
            world.Core.Commands.Post(new AddEntityCommand(world.Id, fpsTextEntity.Id));
            //world.AddEntity(fpsTextEntity);


            var gameViewport = world.Core.GetManager<IEntityMan>().GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();

            //world.Core.Jobs.Execute(new FpsTextUpdateJob(fpsTextEntity));
            gameViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateFpsPos(fpsTextEntity, a));
        }

        private static void UpdateFpsPos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(0.0f, 0.0f);
        }
    }
}

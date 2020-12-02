using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenBreed.Sandbox.Jobs;
using OpenBreed.Sandbox.Worlds;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.CursorCoords
{
    public static class CursorCoordsHelper
    {
        public static void AddToWorld(World world)
        {
            var arial12 = world.Core.Rendering.Fonts.Create("ARIAL", 10);

            var entity = world.Core.Entities.Create();

            entity.Add(PositionComponent.Create(new Vector2(entity.Core.ClientRectangle.Width / 2.0f - 120.0f, -entity.Core.ClientRectangle.Height / 2.0f)));

            var textBuilder = TextComponentBuilder.New(world.Core);
            textBuilder.SetProperty("FontId", arial12.Id);
            textBuilder.SetProperty("Offset", Vector2.Zero);
            textBuilder.SetProperty("Color", Color4.White);
            textBuilder.SetProperty("Text", "Coords: (0.0, 0.0)");
            textBuilder.SetProperty("Order", 100.0f);



            entity.Add(textBuilder.Build());
            world.Core.Commands.Post(new AddEntityCommand(world.Id, entity.Id));
            //world.AddEntity(fpsTextEntity);


            var hudViewport = world.Core.Entities.GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();

            world.Core.Jobs.Execute(new CursorCoordsTextUpdateJob(entity));
            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdatePos(entity, a));
        }

        private static void UpdatePos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(a.Width / 2.0f - 120.0f, -a.Height / 2.0f);
        }
    }
}

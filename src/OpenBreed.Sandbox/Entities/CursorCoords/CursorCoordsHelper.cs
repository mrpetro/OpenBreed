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

namespace OpenBreed.Sandbox.Entities.CursorCoords
{
    public static class CursorCoordsHelper
    {
        public static void AddToWorld(World world)
        {
            var windowClient = world.Core.GetManager<IClientMan>();
            var arial12 = world.Core.GetManager<IFontMan>().Create("ARIAL", 10);

            var entity = world.Core.GetManager<IEntityMan>().Create();

            entity.Add(PositionComponent.Create(new Vector2(windowClient.ClientRectangle.Width / 2.0f - 120.0f, -windowClient.ClientRectangle.Height / 2.0f)));

            var textBuilder = TextComponentBuilderEx.New(world.Core);
            textBuilder.SetFontById(arial12.Id);
            textBuilder.SetOffset(Vector2.Zero);
            textBuilder.SetColor(Color4.White);
            textBuilder.SetText("Coords: (0.0, 0.0)");
            textBuilder.SetOrder(100);



            entity.Add(textBuilder.Build());
            world.Core.Commands.Post(new AddEntityCommand(world.Id, entity.Id));
            //world.AddEntity(fpsTextEntity);


            var hudViewport = world.Core.GetManager<IEntityMan>().GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();

            world.Core.Jobs.Execute(new CursorCoordsTextUpdateJob(entity));
            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdatePos(entity, a));
        }

        private static void UpdatePos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(a.Width / 2.0f - 120.0f, -a.Height / 2.0f);
        }
    }
}

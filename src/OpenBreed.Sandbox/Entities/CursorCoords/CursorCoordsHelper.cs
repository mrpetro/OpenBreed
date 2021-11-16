//using OpenBreed.Core;
//using OpenBreed.Core.Commands;
//using OpenBreed.Wecs.Components.Common;
//using OpenBreed.Wecs.Components.Rendering;
//using OpenBreed.Rendering.Interface;
//using OpenBreed.Wecs.Systems.Rendering.Events;
//using OpenBreed.Sandbox.Jobs;
//using OpenBreed.Sandbox.Worlds;
//using OpenTK;
//using OpenTK.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenBreed.Wecs;
//using OpenBreed.Wecs.Entities;
//using OpenBreed.Wecs.Worlds;
//using OpenBreed.Rendering.Interface.Managers;
//using OpenBreed.Core.Managers;
//using OpenBreed.Common;

//namespace OpenBreed.Sandbox.Entities.CursorCoords
//{
//    public static class CursorCoordsHelper
//    {
//        public static void AddToWorld(ICore core, World world)
//        {
//            var windowClient = core.GetManager<IViewClient>();
//            var arial12 = core.GetManager<IFontMan>().Create("ARIAL", 10);

//            var entity = core.GetManager<IEntityMan>().Create();

//            entity.Add(PositionComponent.Create(new Vector2(windowClient.ClientRectangle.Width / 2.0f - 180.0f, -windowClient.ClientRectangle.Height / 2.0f)));

//            var builderFactory = core.GetManager<IBuilderFactory>();

//            var textBuilder = builderFactory.GetBuilder<TextComponentBuilder>();
//            textBuilder.SetFontById(arial12.Id);
//            textBuilder.SetOffset(Vector2.Zero);
//            textBuilder.SetColor(Color4.White);
//            textBuilder.SetText("Coords: (0.0, 0.0)");
//            textBuilder.SetOrder(100);

//            entity.Add(textBuilder.Build());
//            entity.EnterWorld(world.Id);

//            var hudViewport = core.GetManager<IEntityMan>().GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();

//            core.GetManager<IJobsMan>().Execute(new JohnPositionTextUpdateJob(entityMan, entity));
//            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdatePos(entity, a));
//        }

//        private static void UpdatePos(Entity fpsTextEntity, ViewportResizedEventArgs a)
//        {
//            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(a.Width / 2.0f - 180.0f, -a.Height / 2.0f);
//        }
//    }
//}

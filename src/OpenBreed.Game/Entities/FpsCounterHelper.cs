﻿using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Systems.Rendering.Events;
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
using OpenBreed.Core.Managers;

namespace OpenBreed.Game.Entities
{
    public static class FpsCounterHelper
    {
        public static void AddToWorld(ICore core, World world)
        {
            var arial12 = core.GetManager<IFontMan>().Create("ARIAL", 10);

            var fpsTextEntity = core.GetManager<IEntityMan>().Create();

            fpsTextEntity.Add(PositionComponent.Create(new Vector2(0,0)));

            var textBuilder = core.GetManager<TextComponentBuilder>();
            textBuilder.SetFontById(arial12.Id);
            textBuilder.SetOffset(Vector2.Zero);
            textBuilder.SetColor(Color4.White);
            textBuilder.SetText("FPS: 0.0");
            textBuilder.SetOrder(100);



            fpsTextEntity.Add(textBuilder.Build());
            core.Commands.Post(new AddEntityCommand(world.Id, fpsTextEntity.Id));
            //world.AddEntity(fpsTextEntity);


            var gameViewport = core.GetManager<IEntityMan>().GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();

            var eventsMan = core.GetManager<IEventsMan>();

            //world.Core.Jobs.Execute(new FpsTextUpdateJob(fpsTextEntity));
            eventsMan.Subscribe<ViewportResizedEventArgs>(gameViewport, (s, a) => UpdateFpsPos(fpsTextEntity, a));
        }

        private static void UpdateFpsPos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(0.0f, 0.0f);
        }
    }
}

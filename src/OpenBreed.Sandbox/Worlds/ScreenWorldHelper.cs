using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Components.Common;
using OpenBreed.Systems.Rendering.Commands;
using OpenBreed.Components.Rendering;
using OpenBreed.Rendering.Interface;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Systems.Rendering.Events;
using OpenBreed.Systems.Rendering;
using OpenBreed.Ecsw;
using OpenBreed.Ecsw.Entities;

namespace OpenBreed.Sandbox.Worlds
{
    public static class ScreenWorldHelper
    {
        public const string GAME_VIEWPORT = "GameViewport";
        public const string HUD_VIEWPORT = "HUDViewport";
        public const string TEXT_VIEWPORT = "TextViewport";

        public static void AddSystems(Program core, WorldBuilder builder)
        {
            //Video
            builder.AddSystem(core.VideoSystemsFactory.CreateViewportSystem().Build());
            //builder.AddSystem(core.CreateSpriteSystem().Build());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            //builder.AddSystem(core.CreateTextSystem().Build());
        }

        public static Entity CreateViewportEntity(ICore core, string name, float x, float y, float width, float height, bool drawBackground, bool clipping = true)
        {
            var viewport = core.GetManager<IEntityMan>().Create();
            viewport.Tag = name;

            var vpcBuilder = ViewportComponentBuilderEx.New(core);
            vpcBuilder.SetSize(width, height);
            vpcBuilder.SetDrawBorderFlag(true);
            vpcBuilder.SetDrawBackgroundFlag(drawBackground);
            vpcBuilder.SetClippingFlag(clipping);
            vpcBuilder.SetBackgroundColor(Color4.Black);

            viewport.Add(vpcBuilder.Build());
            viewport.Add(PositionComponent.Create(x, y));

            return viewport;
        }

        public static World CreateWorld(Program core)
        {
            var builder = core.GetManager<IWorldMan>().Create().SetName("ScreenWorld");
            AddSystems(core, builder);

            var world = builder.Build();

            var gameViewport = CreateViewportEntity(core, GAME_VIEWPORT, 32, 32, core.ClientRectangle.Width - 64, core.ClientRectangle.Height - 64, true, true);
            //gameViewport.GetComponent<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            //gameViewport.GetComponent<ViewportComponent>().ScalingType = ViewportScalingType.FitHeightPreserveAspectRatio;
            gameViewport.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            var hudViewport = CreateViewportEntity(core, HUD_VIEWPORT, 0, 0, core.ClientRectangle.Width, core.ClientRectangle.Height, false, true);
            var textViewport = CreateViewportEntity(core, TEXT_VIEWPORT, 0, 0, core.ClientRectangle.Width, core.ClientRectangle.Height, false, true);

            var renderingModule = core.GetModule<IRenderModule>();

            renderingModule.Subscribe<ClientResizedEventArgs>((s, a) => ResizeGameViewport(gameViewport, a));
            renderingModule.Subscribe<ClientResizedEventArgs>((s, a) => ResizeHudViewport(hudViewport, a));
            renderingModule.Subscribe<ClientResizedEventArgs>((s, a) => ResizeTextViewport(hudViewport, a));


            core.Commands.Post(new AddEntityCommand(world.Id, gameViewport.Id));
            core.Commands.Post(new AddEntityCommand(world.Id, hudViewport.Id));
            core.Commands.Post(new AddEntityCommand(world.Id, textViewport.Id));
            //world.AddEntity(gameViewport);
            //world.AddEntity(hudViewport);

            gameViewport.Subscribe<ViewportClickedEventArgs>(OnViewportClick);

            return world;
        }

        private static void OnViewportClick(object sender, ViewportClickedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private static void ResizeGameViewport(Entity viewport, ClientResizedEventArgs args)
        {
            viewport.Core.Commands.Post(new ViewportResizeCommand(viewport.Id, args.Width - 64, args.Height - 64));
        }

        private static void ResizeHudViewport(Entity viewport, ClientResizedEventArgs args)
        {
            viewport.Core.Commands.Post(new ViewportResizeCommand(viewport.Id, args.Width, args.Height));
        }

        private static void ResizeTextViewport(Entity viewport, ClientResizedEventArgs args)
        {
            viewport.Core.Commands.Post(new ViewportResizeCommand(viewport.Id, args.Width, args.Height));
        }
    }
}

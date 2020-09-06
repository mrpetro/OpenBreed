﻿using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Builders;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenTK;
using OpenTK.Graphics;

namespace OpenBreed.Sandbox
{
    public static class Misc
    {
        #region Public Fields

        public const string CRAZY_MOVE_ANIM = "Animations/Misc/CrazyMove";

        #endregion Public Fields

        #region Public Methods

        public static void CreateAnimations(ICore core)
        {
            var updatePosAnim = core.Animations.Create(CRAZY_MOVE_ANIM, 25.0f);
            var updateX = updatePosAnim.AddPart<float>(OnUpdatePosXFrame, 0);
            updateX.AddFrame(5 * 16, 5.0f);
            updateX.AddFrame(0 * 16, 10.0f);
            updateX.AddFrame(1 * 16, 15.0f);
            updateX.AddFrame(4 * 16, 20.0f);
            updateX.AddFrame(0 * 16, 25.0f);

            var updateY = updatePosAnim.AddPart<float>(OnUpdatePosYFrame, 0);
            updateY.AddFrame(0 * 16, 5.0f);
            updateY.AddFrame(1 * 16, 10.0f);
            updateY.AddFrame(5 * 16, 15.0f);
            updateY.AddFrame(2 * 16, 20.0f);
            updateY.AddFrame(0 * 16, 25.0f);
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnUpdatePosXFrame(Entity entity, float nextValue)
        {
            var pos = entity.Get<PositionComponent>();
            pos.Value = new OpenTK.Vector2(nextValue, pos.Value.Y);
        }

        private static void OnUpdatePosYFrame(Entity entity, float nextValue)
        {
            var pos = entity.Get<PositionComponent>();
            pos.Value = new OpenTK.Vector2(pos.Value.X, nextValue);
        }

        public static void AddToWorld(World world)
        {
            var arial12 = world.Core.Rendering.Fonts.Create("ARIAL", 10);

            var crazyMover = world.Core.Entities.Create();



            crazyMover.Add(PositionComponent.Create(0,0));
            crazyMover.Add(new AnimationComponent(10.0f, true, world.Core.Animations.GetByName(CRAZY_MOVE_ANIM).Id, FrameTransition.LinearInterpolation));
            var textBuilder = TextComponentBuilder.New(world.Core);
            textBuilder.SetProperty("FontId", arial12.Id);
            textBuilder.SetProperty("Offset", Vector2.Zero);
            textBuilder.SetProperty("Color", Color4.White);
            textBuilder.SetProperty("Text", "Crazy Moveroooooooooooo");
            textBuilder.SetProperty("Order", 100.0f);

            crazyMover.Add(textBuilder.Build());
            world.Core.Commands.Post(new AddEntityCommand(world.Id, crazyMover.Id));
        }

        #endregion Private Methods
    }
}
﻿using OpenBreed.Animation.Interface;
using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Rendering.Interface;
using OpenTK;
using OpenTK.Graphics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Commands;
using OpenBreed.Rendering.Interface.Managers;

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
            var updatePosAnim = core.GetManager<IClipMan>().CreateClip(CRAZY_MOVE_ANIM, 25.0f);
            var updateX = updatePosAnim.AddTrack<float>(FrameInterpolation.Linear, OnUpdatePosXFrame, 0);
            updateX.AddFrame(5 * 16, 5.0f);
            updateX.AddFrame(0 * 16, 10.0f);
            updateX.AddFrame(1 * 16, 15.0f);
            updateX.AddFrame(4 * 16, 20.0f);
            updateX.AddFrame(0 * 16, 25.0f);

            var updateY = updatePosAnim.AddTrack<float>(FrameInterpolation.Linear, OnUpdatePosYFrame, 0);
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

        public static Entity AddToWorld(ICore core, World world)
        {
            var arial12 = core.GetManager<IFontMan>().Create("ARIAL", 10);

            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\CrazyMover\CrazyMover.xml");
            var crazyMover = core.GetManager<IEntityFactory>().Create(entityTemplate);



            //var crazyMover = world.Core.GetManager<IEntityMan>().CreateFromTemplate("CrazyMover");



            //crazyMover.Add(PositionComponent.Create(0,0));

            //var animCmpBuilder = AnimationComponentBuilder.NewAnimation(world.Core);
            //animCmpBuilder.AddAnimator().SetSpeed(10.0f)
            //                            .SetLoop(true)
            //                            .SetAnimId(world.Core.Animations.GetByName(CRAZY_MOVE_ANIM).Id)
            //                            .SetTransition(FrameTransition.LinearInterpolation);
            //crazyMover.Add(animCmpBuilder.Build());


            //var textBuilder = TextComponentBuilder.New(world.Core);
            //textBuilder.SetProperty("FontId", arial12.Id);
            //textBuilder.SetProperty("Offset", Vector2.Zero);
            //textBuilder.SetProperty("Color", Color4.White);
            //textBuilder.SetProperty("Text", "Crazy Mover");
            //textBuilder.SetProperty("Order", 100.0f);

            //crazyMover.Add(textBuilder.Build());


            core.Commands.Post(new AddEntityCommand(world.Id, crazyMover.Id));

            return crazyMover;
        }

        #endregion Private Methods
    }
}
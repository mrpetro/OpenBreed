using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Sandbox.Entities.Turret
{
    public class TurretHelper
    {
        public TurretHelper(IClipMan<IEntity> clipMan, IEntityFactory entityFactory)
        {
            this.clipMan = clipMan;
            this.entityFactory = entityFactory;
        }

        #region Public Fields

        public const string SPRITE_TURRET = "Atlases/Sprites/Turret";
        private readonly IClipMan<IEntity> clipMan;
        private readonly IEntityFactory entityFactory;

        public void CreateAnimations()
        {
            var animationGuarding0 = clipMan.NewClip("Animations/Turret/Guarding/0", 2.0f);
            animationGuarding0.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 0).AddFrame(0, 2.0f);
            clipMan.Register(animationGuarding0.Build());

            var animationGuarding22_5 = clipMan.NewClip("Animations/Guarding/Guard/22.5", 2.0f);
            animationGuarding22_5.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 1).AddFrame(1, 2.0f);
            clipMan.Register(animationGuarding22_5.Build());

            var animationGuarding45 = clipMan.NewClip("Animations/Turret/Guarding/45", 2.0f);
            animationGuarding45.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 2).AddFrame(2, 2.0f);
            clipMan.Register(animationGuarding45.Build());

            var animationGuarding67_5 = clipMan.NewClip("Animations/Turret/Guarding/67.5", 2.0f);
            animationGuarding67_5.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 3).AddFrame(3, 2.0f);
            clipMan.Register(animationGuarding67_5.Build());

            var animationGuarding90 = clipMan.NewClip("Animations/Guarding/Guard/90", 2.0f);
            animationGuarding90.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 4).AddFrame(4, 2.0f);
            clipMan.Register(animationGuarding90.Build());

            var animationGuarding112_5 = clipMan.NewClip("Animations/Turret/Guarding/112.5", 2.0f);
            animationGuarding112_5.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 5).AddFrame(5, 2.0f);
            clipMan.Register(animationGuarding112_5.Build());

            var animationGuarding135 = clipMan.NewClip("Animations/Turret/Guarding/135", 2.0f);
            animationGuarding135.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 6).AddFrame(6, 2.0f);
            clipMan.Register(animationGuarding135.Build());

            var animationGuarding157_5 = clipMan.NewClip("Animations/Turret/Guarding/157.5", 2.0f);
            animationGuarding157_5.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 7).AddFrame(7, 2.0f);
            clipMan.Register(animationGuarding157_5.Build());

            var animationGuarding180 = clipMan.NewClip("Animations/Turret/Guarding/180", 2.0f);
            animationGuarding180.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 8).AddFrame(8, 2.0f);
            clipMan.Register(animationGuarding180.Build());

            var animationGuarding202_5 = clipMan.NewClip("Animations/Turret/Guarding/202.5", 2.0f);
            animationGuarding202_5.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 9).AddFrame(9, 2.0f);
            clipMan.Register(animationGuarding202_5.Build());

            var animationGuarding225 = clipMan.NewClip("Animations/Guarding/Guard/225", 2.0f);
            animationGuarding225.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 10).AddFrame(10, 2.0f);
            clipMan.Register(animationGuarding225.Build());

            var animationGuarding247_5 = clipMan.NewClip("Animations/Turret/Guarding/247.5", 2.0f);
            animationGuarding247_5.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 11).AddFrame(11, 2.0f);
            clipMan.Register(animationGuarding247_5.Build());

            var animationGuarding270 = clipMan.NewClip("Animations/Turret/Guarding/270", 2.0f);
            animationGuarding270.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 12).AddFrame(12, 2.0f);
            clipMan.Register(animationGuarding270.Build());

            var animationGuarding292_5 = clipMan.NewClip("Animations/Guarding/Guard/292.5", 2.0f);
            animationGuarding292_5.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 13).AddFrame(13, 2.0f);
            clipMan.Register(animationGuarding292_5.Build());

            var animationGuarding315 = clipMan.NewClip("Animations/Turret/Guarding/315", 2.0f);
            animationGuarding315.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 14).AddFrame(14, 2.0f);
            clipMan.Register(animationGuarding315.Build());

            var animationGuarding337_5 = clipMan.NewClip("Animations/Turret/Guarding/337.5", 2.0f);
            animationGuarding337_5.AddTrack<int>("SpriteId", FrameInterpolation.None, OnFrameUpdate, 15).AddFrame(15, 2.0f);
            clipMan.Register(animationGuarding337_5.Build());
        }
        private void OnStop()
        {
            Console.WriteLine("Rotation -> Stopped");
        }

        private void OnFrameUpdate(IEntity entity, int nextValue)
        {
            //entity.Core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        public IEntity Create(Vector2 pos)
        {
            var entity = entityFactory.Create(@"Entities\Turret\Turret")
                .SetParameter("startX", pos.X)
                .SetParameter("startY", pos.Y)
                .Build();

            return entity;
        }


        #endregion Public Fields
    }
}
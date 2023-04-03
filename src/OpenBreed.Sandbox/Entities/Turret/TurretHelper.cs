using OpenBreed.Common.Tools;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenTK;
using System;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs;
using OpenBreed.Fsm;
using OpenBreed.Core.Managers;
using OpenBreed.Common.Tools.Xml;
using OpenTK.Mathematics;

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
            var animationGuarding0 = clipMan.CreateClip("Animations/Turret/Guarding/0", 2.0f);
            animationGuarding0.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 0).AddFrame(0, 2.0f);
            var animationGuarding22_5 = clipMan.CreateClip("Animations/Guarding/Guard/22.5", 2.0f);
            animationGuarding22_5.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 1).AddFrame(1, 2.0f);
            var animationGuarding45 = clipMan.CreateClip("Animations/Turret/Guarding/45", 2.0f);
            animationGuarding45.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 2).AddFrame(2, 2.0f);
            var animationGuarding67_5 = clipMan.CreateClip("Animations/Turret/Guarding/67.5", 2.0f);
            animationGuarding67_5.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 3).AddFrame(3, 2.0f);
            var animationGuarding90 = clipMan.CreateClip("Animations/Guarding/Guard/90", 2.0f);
            animationGuarding90.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 4).AddFrame(4, 2.0f);
            var animationGuarding112_5 = clipMan.CreateClip("Animations/Turret/Guarding/112.5", 2.0f);
            animationGuarding112_5.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 5).AddFrame(5, 2.0f);
            var animationGuarding135 = clipMan.CreateClip("Animations/Turret/Guarding/135", 2.0f);
            animationGuarding135.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 6).AddFrame(6, 2.0f);
            var animationGuarding157_5 = clipMan.CreateClip("Animations/Turret/Guarding/157.5", 2.0f);
            animationGuarding157_5.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 7).AddFrame(7, 2.0f);
            var animationGuarding180 = clipMan.CreateClip("Animations/Turret/Guarding/180", 2.0f);
            animationGuarding180.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 8).AddFrame(8, 2.0f);
            var animationGuarding202_5 = clipMan.CreateClip("Animations/Turret/Guarding/202.5", 2.0f);
            animationGuarding202_5.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 9).AddFrame(9, 2.0f);
            var animationGuarding225 = clipMan.CreateClip("Animations/Guarding/Guard/225", 2.0f);
            animationGuarding225.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 10).AddFrame(10, 2.0f);
            var animationGuarding247_5 = clipMan.CreateClip("Animations/Turret/Guarding/247.5", 2.0f);
            animationGuarding247_5.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 11).AddFrame(11, 2.0f);
            var animationGuarding270 = clipMan.CreateClip("Animations/Turret/Guarding/270", 2.0f);
            animationGuarding270.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 12).AddFrame(12, 2.0f);
            var animationGuarding292_5 = clipMan.CreateClip("Animations/Guarding/Guard/292.5", 2.0f);
            animationGuarding292_5.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 13).AddFrame(13, 2.0f);
            var animationGuarding315 = clipMan.CreateClip("Animations/Turret/Guarding/315", 2.0f);
            animationGuarding315.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 14).AddFrame(14, 2.0f);
            var animationGuarding337_5 = clipMan.CreateClip("Animations/Turret/Guarding/337.5", 2.0f);
            animationGuarding337_5.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 15).AddFrame(15, 2.0f);
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
using OpenBreed.Common.Tools;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Physics.Generic;
using OpenBreed.Physics.Generic.Helpers;
using OpenBreed.Physics.Interface;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenTK;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Input.Generic;
using OpenBreed.Input.Interface;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public  class ActorHelper
    {
        public ActorHelper(ICore core)
        {
            this.core = core;
        }

        #region Public Fields

        public const string SPRITE_ARROW = "Atlases/Sprites/Arrow";
        private readonly ICore core;

        #endregion Public Fields

        #region Public Methods

        public void RegisterCollisionPairs()
        {
            var collisionMan = core.GetManager<ICollisionMan>();

            collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.StaticObstacle, Dynamic2StaticCallback);
        }

        public void CreateAnimations()
        {
            var animations = core.GetManager<IClipMan>();

            var animationStandingRight = animations.CreateClip("Animations/Actor/Standing/Right", 2.0f);
            animationStandingRight.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 0).AddFrame(0, 2.0f);
            var animationStandingRightDown = animations.CreateClip("Animations/Actor/Standing/RightDown", 2.0f);
            animationStandingRightDown.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 1).AddFrame(1, 2.0f);
            var animationStandingDown = animations.CreateClip("Animations/Actor/Standing/Down", 2.0f);
            animationStandingDown.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 2).AddFrame(2, 2.0f);
            var animationStandingDownLeft = animations.CreateClip("Animations/Actor/Standing/DownLeft", 2.0f);
            animationStandingDownLeft.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 3).AddFrame(3, 2.0f);
            var animationStandingLeft = animations.CreateClip("Animations/Actor/Standing/Left", 2.0f);
            animationStandingLeft.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 4).AddFrame(4, 2.0f);
            var animationStandingLeftUp = animations.CreateClip("Animations/Actor/Standing/LeftUp", 2.0f);
            animationStandingLeftUp.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 5).AddFrame(5, 2.0f);
            var animationStandingUp = animations.CreateClip("Animations/Actor/Standing/Up", 2.0f);
            animationStandingUp.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 6).AddFrame(6, 2.0f);
            var animationStandingUpRight = animations.CreateClip("Animations/Actor/Standing/UpRight", 2.0f);
            animationStandingUpRight.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 7).AddFrame(7, 2.0f);

            var animationWalkingRight = animations.CreateClip("Animations/Actor/Walking/Right", 5.0f);
            var awrp = animationWalkingRight.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 0);
            awrp.AddFrame(0, 1.0f);
            awrp.AddFrame(8, 2.0f);
            awrp.AddFrame(16, 3.0f);
            awrp.AddFrame(24, 4.0f);
            awrp.AddFrame(32, 5.0f);
            var animationWalkingRightDown = animations.CreateClip("Animations/Actor/Walking/RightDown", 5.0f);
            var awrd = animationWalkingRightDown.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 1);
            awrd.AddFrame(1, 1.0f);
            awrd.AddFrame(9, 2.0f);
            awrd.AddFrame(17, 3.0f);
            awrd.AddFrame(25, 4.0f);
            awrd.AddFrame(33, 5.0f);
            var animationWalkingDown = animations.CreateClip("Animations/Actor/Walking/Down", 5.0f);
            var awd = animationWalkingDown.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 2);
            awd.AddFrame(2, 1.0f);
            awd.AddFrame(10, 2.0f);
            awd.AddFrame(18, 3.0f);
            awd.AddFrame(26, 4.0f);
            awd.AddFrame(34, 5.0f);
            var animationWalkingDownLeft = animations.CreateClip("Animations/Actor/Walking/DownLeft", 5.0f);
            var awdl = animationWalkingDownLeft.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 3);
            awdl.AddFrame(3, 1.0f);
            awdl.AddFrame(11, 2.0f);
            awdl.AddFrame(19, 3.0f);
            awdl.AddFrame(27, 4.0f);
            awdl.AddFrame(35, 5.0f);
            var animationWalkingLeft = animations.CreateClip("Animations/Actor/Walking/Left", 5.0f);
            var awl = animationWalkingLeft.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 4);
            awl.AddFrame(4, 1.0f);
            awl.AddFrame(12, 2.0f);
            awl.AddFrame(20, 3.0f);
            awl.AddFrame(28, 4.0f);
            awl.AddFrame(36, 5.0f);
            var animationWalkingLeftUp = animations.CreateClip("Animations/Actor/Walking/LeftUp", 5.0f);
            var awlu = animationWalkingLeftUp.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 5);
            awlu.AddFrame(5, 1.0f);
            awlu.AddFrame(13, 2.0f);
            awlu.AddFrame(21, 3.0f);
            awlu.AddFrame(29, 4.0f);
            awlu.AddFrame(37, 5.0f);
            var animationWalkingUp = animations.CreateClip("Animations/Actor/Walking/Up", 5.0f);
            var awu = animationWalkingUp.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 6);
            awu.AddFrame(6, 1.0f);
            awu.AddFrame(14, 2.0f);
            awu.AddFrame(22, 3.0f);
            awu.AddFrame(30, 4.0f);
            awu.AddFrame(38, 5.0f);
            var animationWalkingUpRight = animations.CreateClip("Animations/Actor/Walking/UpRight", 5.0f);
            var awur = animationWalkingUpRight.AddTrack<int>(FrameInterpolation.None, OnFrameUpdate, 7);
            awur.AddFrame(7, 1.0f);
            awur.AddFrame(15, 2.0f);
            awur.AddFrame(23, 3.0f);
            awur.AddFrame(31, 4.0f);
            awur.AddFrame(39, 5.0f);
        }

        public Entity CreatePlayerActor(Vector2 pos)
        {
            var playersMan = core.GetManager<IPlayersMan>();

            var actor = CreateActor(pos);

            var p1 = playersMan.GetByName("P1");

            actor.Add(new WalkingInputComponent(p1.Id, 0));
            actor.Add(new AttackInputComponent(p1.Id, 0));
            actor.Add(new WalkingControlComponent());
            actor.Add(new AttackControlComponent());

            return actor;
        }

        public Entity CreateActor(Vector2 pos)
        {
            //var actor = core.GetManager<IEntityMan>().Create();

            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Actor\Arrow.xml");
            var actor = core.GetManager<IEntityFactory>().Create(entityTemplate);
            //var actor = core.GetManager<IEntityMan>().CreateFromTemplate("Arrow");


            actor.Add(new AngularVelocityComponent(0));
            actor.Add(new AngularThrustComponent(0));
            actor.Add(new CollisionComponent(ColliderTypes.ActorBody));
            //actor.Add(new InventoryComponent(new Bag[] { new Bag("Backpack") }));
            //actor.Add(new EquipmentComponent(new Slot[] { new Slot("Torso"), new Slot("Hands") }));
            //actor.Add(AxisAlignedBoxShape.Create(0, 0, 32, 32));
            actor.Add(new FollowerComponent());
            actor.Get<PositionComponent>().Value = pos;

            //actor.Subscribe<CollisionEventArgs>(OnCollision);

            return actor;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnFrameUpdate(Entity entity, int nextValue)
        {
            core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        private void Dynamic2StaticCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        {
            DynamicHelper.ResolveVsStatic(entityA, entityB, projection);
        }

        //private static void OnCollision(object sender, CollisionEventArgs args)
        //{
        //    var entity = (Entity)sender;
        //    var body = args.Entity.TryGet<BodyComponent>();

        //    var type = body.Tag;

        //    switch (type)
        //    {
        //        case "Static":
        //            DynamicHelper.ResolveVsStatic(entity, args.Entity, args.Projection);
        //            return;

        //        default:
        //            break;
        //    }
        //}

        #endregion Private Methods
    }
}
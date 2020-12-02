using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Physics;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Sandbox.Entities.Actor.States.Attacking;
using OpenBreed.Sandbox.Entities.Actor.States.Movement;
using OpenBreed.Sandbox.Entities.Actor.States.Rotation;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.CodeDom;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorHelper
    {
        #region Public Fields

        public const string SPRITE_ARROW = "Atlases/Sprites/Arrow";

        #endregion Public Fields

        #region Public Methods

        public static void RegisterCollisionPairs(ICore core)
        {
            var collisionMan = core.GetModule<PhysicsModule>().Collisions;

            collisionMan.RegisterCollisionPair(ColliderTypes.ActorBody, ColliderTypes.StaticObstacle, Dynamic2StaticCallback);
        }

        public static void CreateAnimations(ICore core)
        {
            var animationStandingRight = core.Animations.Create("Animations/Actor/Standing/Right", 2.0f);
            animationStandingRight.AddPart<int>(OnFrameUpdate, 0).AddFrame(0, 2.0f);
            var animationStandingRightDown = core.Animations.Create("Animations/Actor/Standing/RightDown", 2.0f);
            animationStandingRightDown.AddPart<int>(OnFrameUpdate, 1).AddFrame(1, 2.0f);
            var animationStandingDown = core.Animations.Create("Animations/Actor/Standing/Down", 2.0f);
            animationStandingDown.AddPart<int>(OnFrameUpdate, 2).AddFrame(2, 2.0f);
            var animationStandingDownLeft = core.Animations.Create("Animations/Actor/Standing/DownLeft", 2.0f);
            animationStandingDownLeft.AddPart<int>(OnFrameUpdate, 3).AddFrame(3, 2.0f);
            var animationStandingLeft = core.Animations.Create("Animations/Actor/Standing/Left", 2.0f);
            animationStandingLeft.AddPart<int>(OnFrameUpdate, 4).AddFrame(4, 2.0f);
            var animationStandingLeftUp = core.Animations.Create("Animations/Actor/Standing/LeftUp", 2.0f);
            animationStandingLeftUp.AddPart<int>(OnFrameUpdate, 5).AddFrame(5, 2.0f);
            var animationStandingUp = core.Animations.Create("Animations/Actor/Standing/Up", 2.0f);
            animationStandingUp.AddPart<int>(OnFrameUpdate, 6).AddFrame(6, 2.0f);
            var animationStandingUpRight = core.Animations.Create("Animations/Actor/Standing/UpRight", 2.0f);
            animationStandingUpRight.AddPart<int>(OnFrameUpdate, 7).AddFrame(7, 2.0f);

            var animationWalkingRight = core.Animations.Create("Animations/Actor/Walking/Right", 5.0f);
            var awrp = animationWalkingRight.AddPart<int>(OnFrameUpdate, 0);
            awrp.AddFrame(0, 1.0f);
            awrp.AddFrame(8, 2.0f);
            awrp.AddFrame(16, 3.0f);
            awrp.AddFrame(24, 4.0f);
            awrp.AddFrame(32, 5.0f);
            var animationWalkingRightDown = core.Animations.Create("Animations/Actor/Walking/RightDown", 5.0f);
            var awrd = animationWalkingRightDown.AddPart<int>(OnFrameUpdate, 1);
            awrd.AddFrame(1, 1.0f);
            awrd.AddFrame(9, 2.0f);
            awrd.AddFrame(17, 3.0f);
            awrd.AddFrame(25, 4.0f);
            awrd.AddFrame(33, 5.0f);
            var animationWalkingDown = core.Animations.Create("Animations/Actor/Walking/Down", 5.0f);
            var awd = animationWalkingDown.AddPart<int>(OnFrameUpdate, 2);
            awd.AddFrame(2, 1.0f);
            awd.AddFrame(10, 2.0f);
            awd.AddFrame(18, 3.0f);
            awd.AddFrame(26, 4.0f);
            awd.AddFrame(34, 5.0f);
            var animationWalkingDownLeft = core.Animations.Create("Animations/Actor/Walking/DownLeft", 5.0f);
            var awdl = animationWalkingDownLeft.AddPart<int>(OnFrameUpdate, 3);
            awdl.AddFrame(3, 1.0f);
            awdl.AddFrame(11, 2.0f);
            awdl.AddFrame(19, 3.0f);
            awdl.AddFrame(27, 4.0f);
            awdl.AddFrame(35, 5.0f);
            var animationWalkingLeft = core.Animations.Create("Animations/Actor/Walking/Left", 5.0f);
            var awl = animationWalkingLeft.AddPart<int>(OnFrameUpdate, 4);
            awl.AddFrame(4, 1.0f);
            awl.AddFrame(12, 2.0f);
            awl.AddFrame(20, 3.0f);
            awl.AddFrame(28, 4.0f);
            awl.AddFrame(36, 5.0f);
            var animationWalkingLeftUp = core.Animations.Create("Animations/Actor/Walking/LeftUp", 5.0f);
            var awlu = animationWalkingLeftUp.AddPart<int>(OnFrameUpdate, 5);
            awlu.AddFrame(5, 1.0f);
            awlu.AddFrame(13, 2.0f);
            awlu.AddFrame(21, 3.0f);
            awlu.AddFrame(29, 4.0f);
            awlu.AddFrame(37, 5.0f);
            var animationWalkingUp = core.Animations.Create("Animations/Actor/Walking/Up", 5.0f);
            var awu = animationWalkingUp.AddPart<int>(OnFrameUpdate, 6);
            awu.AddFrame(6, 1.0f);
            awu.AddFrame(14, 2.0f);
            awu.AddFrame(22, 3.0f);
            awu.AddFrame(30, 4.0f);
            awu.AddFrame(38, 5.0f);
            var animationWalkingUpRight = core.Animations.Create("Animations/Actor/Walking/UpRight", 5.0f);
            var awur = animationWalkingUpRight.AddPart<int>(OnFrameUpdate, 7);
            awur.AddFrame(7, 1.0f);
            awur.AddFrame(15, 2.0f);
            awur.AddFrame(23, 3.0f);
            awur.AddFrame(31, 4.0f);
            awur.AddFrame(39, 5.0f);
        }

        public static Entity CreateActor(ICore core, Vector2 pos)
        {
            //var actor = core.Entities.Create();

            var actor = core.Entities.CreateFromTemplate("Arrow");
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

        private static void OnFrameUpdate(Entity entity, int nextValue)
        {
            entity.Core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        private static void Dynamic2StaticCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
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
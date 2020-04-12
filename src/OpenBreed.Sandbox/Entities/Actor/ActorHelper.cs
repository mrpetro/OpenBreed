using OpenBreed.Core;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Entities.Actor.States;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Sandbox.Entities.Actor.States.Movement;
using OpenBreed.Sandbox.Entities.Actor.States.Attacking;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenBreed.Sandbox.Entities.Actor.States.Rotation;
using OpenBreed.Core.Common.Components;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorHelper
    {
        public static void CreateAnimations(ICore core)
        {
            var animationStandingRight = core.Animations.Create<int>("Animations/Actor/Standing/Right");
            animationStandingRight.AddFrame(0, 2.0f);
            var animationStandingRightDown = core.Animations.Create<int>("Animations/Actor/Standing/RightDown");
            animationStandingRightDown.AddFrame(1, 2.0f);
            var animationStandingDown = core.Animations.Create<int>("Animations/Actor/Standing/Down");
            animationStandingDown.AddFrame(2, 2.0f);
            var animationStandingDownLeft = core.Animations.Create<int>("Animations/Actor/Standing/DownLeft");
            animationStandingDownLeft.AddFrame(3, 2.0f);
            var animationStandingLeft = core.Animations.Create<int>("Animations/Actor/Standing/Left");
            animationStandingLeft.AddFrame(4, 2.0f);
            var animationStandingLeftUp = core.Animations.Create<int>("Animations/Actor/Standing/LeftUp");
            animationStandingLeftUp.AddFrame(5, 2.0f);
            var animationStandingUp = core.Animations.Create<int>("Animations/Actor/Standing/Up");
            animationStandingUp.AddFrame(6, 2.0f);
            var animationStandingUpRight = core.Animations.Create<int>("Animations/Actor/Standing/UpRight");
            animationStandingUpRight.AddFrame(7, 2.0f);

            var animationWalkingRight = core.Animations.Create<int>("Animations/Actor/Walking/Right");
            animationWalkingRight.AddFrame(0, 1.0f);
            animationWalkingRight.AddFrame(8, 1.0f);
            animationWalkingRight.AddFrame(16, 1.0f);
            animationWalkingRight.AddFrame(24, 1.0f);
            animationWalkingRight.AddFrame(32, 1.0f);
            var animationWalkingRightDown = core.Animations.Create<int>("Animations/Actor/Walking/RightDown");
            animationWalkingRightDown.AddFrame(1, 1.0f);
            animationWalkingRightDown.AddFrame(9, 1.0f);
            animationWalkingRightDown.AddFrame(17, 1.0f);
            animationWalkingRightDown.AddFrame(25, 1.0f);
            animationWalkingRightDown.AddFrame(33, 1.0f);
            var animationWalkingDown = core.Animations.Create<int>("Animations/Actor/Walking/Down");
            animationWalkingDown.AddFrame(2, 1.0f);
            animationWalkingDown.AddFrame(10, 1.0f);
            animationWalkingDown.AddFrame(18, 1.0f);
            animationWalkingDown.AddFrame(26, 1.0f);
            animationWalkingDown.AddFrame(34, 1.0f);
            var animationWalkingDownLeft = core.Animations.Create<int>("Animations/Actor/Walking/DownLeft");
            animationWalkingDownLeft.AddFrame(3, 1.0f);
            animationWalkingDownLeft.AddFrame(11, 1.0f);
            animationWalkingDownLeft.AddFrame(19, 1.0f);
            animationWalkingDownLeft.AddFrame(27, 1.0f);
            animationWalkingDownLeft.AddFrame(35, 1.0f);
            var animationWalkingLeft = core.Animations.Create<int>("Animations/Actor/Walking/Left");
            animationWalkingLeft.AddFrame(4, 1.0f);
            animationWalkingLeft.AddFrame(12, 1.0f);
            animationWalkingLeft.AddFrame(20, 1.0f);
            animationWalkingLeft.AddFrame(28, 1.0f);
            animationWalkingLeft.AddFrame(36, 1.0f);
            var animationWalkingLeftUp = core.Animations.Create<int>("Animations/Actor/Walking/LeftUp");
            animationWalkingLeftUp.AddFrame(5, 1.0f);
            animationWalkingLeftUp.AddFrame(13, 1.0f);
            animationWalkingLeftUp.AddFrame(21, 1.0f);
            animationWalkingLeftUp.AddFrame(29, 1.0f);
            animationWalkingLeftUp.AddFrame(37, 1.0f);
            var animationWalkingUp = core.Animations.Create<int>("Animations/Actor/Walking/Up");
            animationWalkingUp.AddFrame(6, 1.0f);
            animationWalkingUp.AddFrame(14, 1.0f);
            animationWalkingUp.AddFrame(22, 1.0f);
            animationWalkingUp.AddFrame(30, 1.0f);
            animationWalkingUp.AddFrame(38, 1.0f);
            var animationWalkingUpRight = core.Animations.Create<int>("Animations/Actor/Walking/UpRight");
            animationWalkingUpRight.AddFrame(7, 1.0f);
            animationWalkingUpRight.AddFrame(15, 1.0f);
            animationWalkingUpRight.AddFrame(23, 1.0f);
            animationWalkingUpRight.AddFrame(31, 1.0f);
            animationWalkingUpRight.AddFrame(39, 1.0f);
        }

        public static IEntity CreateActor(ICore core, Vector2 pos)
        {
            //var actor = core.Entities.Create();

            var actor = core.Entities.CreateFromTemplate("Arrow");

            //actor.Add(new InventoryComponent(new Bag[] { new Bag("Backpack") }));
            //actor.Add(new EquipmentComponent(new Slot[] { new Slot("Torso"), new Slot("Hands") }));
            //actor.Add(AxisAlignedBoxShape.Create(0, 0, 32, 32));


            actor.GetComponent<PositionComponent>().Value = pos;

            actor.Subscribe<CollisionEventArgs>(OnCollision);

            return actor;
        }

        private static void OnCollision(object sender, CollisionEventArgs args)
        {
            var entity = (IEntity)sender;
            var body = args.Entity.TryGetComponent<BodyComponent>();

            var type = body.Tag;

            switch (type)
            {
                case "Static":
                    DynamicHelper.ResolveVsStatic(entity, args.Entity, args.Projection);
                    return;
                default:
                    break;
            }
        }

        public static StateMachine<AttackingState, AttackingImpulse> CreateAttackingFSM(IEntity entity)
        {
            var stateMachine = entity.AddFsm<AttackingState, AttackingImpulse>();

            stateMachine.AddState(new ShootingState());
            stateMachine.AddState(new States.Attacking.IdleState());
            stateMachine.AddState(new CooldownState());

            stateMachine.AddTransition(AttackingState.Shooting, AttackingImpulse.Stop, AttackingState.Idle);
            stateMachine.AddTransition(AttackingState.Shooting, AttackingImpulse.Wait, AttackingState.Cooldown);
            stateMachine.AddTransition(AttackingState.Cooldown, AttackingImpulse.Stop, AttackingState.Idle);
            stateMachine.AddTransition(AttackingState.Cooldown, AttackingImpulse.Shoot, AttackingState.Shooting);
            stateMachine.AddTransition(AttackingState.Idle, AttackingImpulse.Shoot, AttackingState.Shooting);

            return stateMachine;
        }

        public static StateMachine<RotationState, RotationImpulse> CreateRotationFSM(IEntity entity)
        {
            var stateMachine = entity.AddFsm<RotationState, RotationImpulse>();

            stateMachine.AddState(new States.Rotation.IdleState());
            stateMachine.AddState(new RotatingState("Animations/Actor"));

            stateMachine.AddTransition(RotationState.Rotating, RotationImpulse.Stop , RotationState.Idle);
            stateMachine.AddTransition(RotationState.Idle, RotationImpulse.Rotate, RotationState.Rotating);

            stateMachine.AddOnEnterState(RotationState.Idle, RotationImpulse.Stop, OnStop);

            return stateMachine;
        }

        private static void OnStop()
        {
            //Console.WriteLine("Rotation -> Stopped");
        }

        public static StateMachine<MovementState, MovementImpulse> CreateMovementFSM(IEntity entity)
        {
            var stateMachine = entity.AddFsm<MovementState, MovementImpulse>();

            stateMachine.AddState(new StandingState("Animations/Actor"));
            stateMachine.AddState(new WalkingState("Animations/Actor"));

            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Stop, MovementState.Standing);
            stateMachine.AddTransition(MovementState.Standing, MovementImpulse.Walk, MovementState.Walking);

            return stateMachine;
        }
    }
}

using OpenBreed.Core;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Sandbox.Entities.Actor.States.Attacking;
using OpenBreed.Sandbox.Entities.Actor.States.Movement;
using OpenBreed.Sandbox.Entities.Actor.States.Rotation;
using OpenTK;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorHelper
    {
        #region Public Fields

        public const string SPRITE_ARROW = "Atlases/Sprites/Arrow";

        #endregion Public Fields

        #region Public Methods

        public static void CreateAnimations(ICore core)
        {
            var animationStandingRight = core.Animations.Create<int>("Animations/Actor/Standing/Right", OnFrameUpdate);
            animationStandingRight.AddFrame(0, 2.0f);
            var animationStandingRightDown = core.Animations.Create<int>("Animations/Actor/Standing/RightDown", OnFrameUpdate);
            animationStandingRightDown.AddFrame(1, 2.0f);
            var animationStandingDown = core.Animations.Create<int>("Animations/Actor/Standing/Down", OnFrameUpdate);
            animationStandingDown.AddFrame(2, 2.0f);
            var animationStandingDownLeft = core.Animations.Create<int>("Animations/Actor/Standing/DownLeft", OnFrameUpdate);
            animationStandingDownLeft.AddFrame(3, 2.0f);
            var animationStandingLeft = core.Animations.Create<int>("Animations/Actor/Standing/Left", OnFrameUpdate);
            animationStandingLeft.AddFrame(4, 2.0f);
            var animationStandingLeftUp = core.Animations.Create<int>("Animations/Actor/Standing/LeftUp", OnFrameUpdate);
            animationStandingLeftUp.AddFrame(5, 2.0f);
            var animationStandingUp = core.Animations.Create<int>("Animations/Actor/Standing/Up", OnFrameUpdate);
            animationStandingUp.AddFrame(6, 2.0f);
            var animationStandingUpRight = core.Animations.Create<int>("Animations/Actor/Standing/UpRight", OnFrameUpdate);
            animationStandingUpRight.AddFrame(7, 2.0f);

            var animationWalkingRight = core.Animations.Create<int>("Animations/Actor/Walking/Right", OnFrameUpdate);
            animationWalkingRight.AddFrame(0, 1.0f);
            animationWalkingRight.AddFrame(8, 1.0f);
            animationWalkingRight.AddFrame(16, 1.0f);
            animationWalkingRight.AddFrame(24, 1.0f);
            animationWalkingRight.AddFrame(32, 1.0f);
            var animationWalkingRightDown = core.Animations.Create<int>("Animations/Actor/Walking/RightDown", OnFrameUpdate);
            animationWalkingRightDown.AddFrame(1, 1.0f);
            animationWalkingRightDown.AddFrame(9, 1.0f);
            animationWalkingRightDown.AddFrame(17, 1.0f);
            animationWalkingRightDown.AddFrame(25, 1.0f);
            animationWalkingRightDown.AddFrame(33, 1.0f);
            var animationWalkingDown = core.Animations.Create<int>("Animations/Actor/Walking/Down", OnFrameUpdate);
            animationWalkingDown.AddFrame(2, 1.0f);
            animationWalkingDown.AddFrame(10, 1.0f);
            animationWalkingDown.AddFrame(18, 1.0f);
            animationWalkingDown.AddFrame(26, 1.0f);
            animationWalkingDown.AddFrame(34, 1.0f);
            var animationWalkingDownLeft = core.Animations.Create<int>("Animations/Actor/Walking/DownLeft", OnFrameUpdate);
            animationWalkingDownLeft.AddFrame(3, 1.0f);
            animationWalkingDownLeft.AddFrame(11, 1.0f);
            animationWalkingDownLeft.AddFrame(19, 1.0f);
            animationWalkingDownLeft.AddFrame(27, 1.0f);
            animationWalkingDownLeft.AddFrame(35, 1.0f);
            var animationWalkingLeft = core.Animations.Create<int>("Animations/Actor/Walking/Left", OnFrameUpdate);
            animationWalkingLeft.AddFrame(4, 1.0f);
            animationWalkingLeft.AddFrame(12, 1.0f);
            animationWalkingLeft.AddFrame(20, 1.0f);
            animationWalkingLeft.AddFrame(28, 1.0f);
            animationWalkingLeft.AddFrame(36, 1.0f);
            var animationWalkingLeftUp = core.Animations.Create<int>("Animations/Actor/Walking/LeftUp", OnFrameUpdate);
            animationWalkingLeftUp.AddFrame(5, 1.0f);
            animationWalkingLeftUp.AddFrame(13, 1.0f);
            animationWalkingLeftUp.AddFrame(21, 1.0f);
            animationWalkingLeftUp.AddFrame(29, 1.0f);
            animationWalkingLeftUp.AddFrame(37, 1.0f);
            var animationWalkingUp = core.Animations.Create<int>("Animations/Actor/Walking/Up", OnFrameUpdate);
            animationWalkingUp.AddFrame(6, 1.0f);
            animationWalkingUp.AddFrame(14, 1.0f);
            animationWalkingUp.AddFrame(22, 1.0f);
            animationWalkingUp.AddFrame(30, 1.0f);
            animationWalkingUp.AddFrame(38, 1.0f);
            var animationWalkingUpRight = core.Animations.Create<int>("Animations/Actor/Walking/UpRight", OnFrameUpdate);
            animationWalkingUpRight.AddFrame(7, 1.0f);
            animationWalkingUpRight.AddFrame(15, 1.0f);
            animationWalkingUpRight.AddFrame(23, 1.0f);
            animationWalkingUpRight.AddFrame(31, 1.0f);
            animationWalkingUpRight.AddFrame(39, 1.0f);
        }

        public static Entity CreateActor(ICore core, Vector2 pos)
        {
            //var actor = core.Entities.Create();

            var actor = core.Entities.CreateFromTemplate("Arrow");
            //actor.Add(new TimerComponent());
            //actor.Add(new InventoryComponent(new Bag[] { new Bag("Backpack") }));
            //actor.Add(new EquipmentComponent(new Slot[] { new Slot("Torso"), new Slot("Hands") }));
            //actor.Add(AxisAlignedBoxShape.Create(0, 0, 32, 32));
            actor.Add(new FollowerComponent());
            actor.Get<PositionComponent>().Value = pos;

            actor.Subscribe<CollisionEventArgs>(OnCollision);

            return actor;
        }

        public static void CreateAttackingFsm(ICore core)
        {
            var stateMachine = core.StateMachines.Create<AttackingState, AttackingImpulse>("Actor.Attacking");

            stateMachine.AddState(new States.Attacking.ShootingState());
            stateMachine.AddState(new States.Attacking.IdleState());
            stateMachine.AddState(new States.Attacking.CooldownState());

            stateMachine.AddTransition(AttackingState.Shooting, AttackingImpulse.Stop, AttackingState.Idle);
            stateMachine.AddTransition(AttackingState.Shooting, AttackingImpulse.Wait, AttackingState.Cooldown);
            stateMachine.AddTransition(AttackingState.Cooldown, AttackingImpulse.Stop, AttackingState.Idle);
            stateMachine.AddTransition(AttackingState.Cooldown, AttackingImpulse.Shoot, AttackingState.Shooting);
            stateMachine.AddTransition(AttackingState.Idle, AttackingImpulse.Shoot, AttackingState.Shooting);
        }

        public static void CreateRotationFsm(ICore core)
        {
            var stateMachine = core.StateMachines.Create<RotationState, RotationImpulse>("Actor.Rotation");

            stateMachine.AddState(new States.Rotation.IdleState());
            stateMachine.AddState(new States.Rotation.RotatingState());

            stateMachine.AddTransition(RotationState.Rotating, RotationImpulse.Stop, RotationState.Idle);
            stateMachine.AddTransition(RotationState.Idle, RotationImpulse.Rotate, RotationState.Rotating);

            stateMachine.AddOnEnterState(RotationState.Idle, RotationImpulse.Stop, OnStop);
        }

        public static void CreateMovementFsm(ICore core)
        {
            var stateMachine = core.StateMachines.Create<MovementState, MovementImpulse>("Actor.Movement");

            stateMachine.AddState(new StandingState());
            stateMachine.AddState(new WalkingState());

            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Stop, MovementState.Standing);
            stateMachine.AddTransition(MovementState.Standing, MovementImpulse.Walk, MovementState.Walking);
            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Walk, MovementState.Walking);
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnFrameUpdate(Entity entity, int nextValue)
        {
            entity.Core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        private static void OnCollision(object sender, CollisionEventArgs args)
        {
            var entity = (Entity)sender;
            var body = args.Entity.TryGet<BodyComponent>();

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

        private static void OnStop()
        {
            //Console.WriteLine("Rotation -> Stopped");
        }

        #endregion Private Methods
    }
}
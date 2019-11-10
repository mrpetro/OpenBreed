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
            var animationStandingRight = core.Animations.Anims.Create<int>("Animations/Actor/Standing/Right");
            animationStandingRight.AddFrame(0, 2.0f);
            var animationStandingRightDown = core.Animations.Anims.Create<int>("Animations/Actor/Standing/RightDown");
            animationStandingRightDown.AddFrame(1, 2.0f);
            var animationStandingDown = core.Animations.Anims.Create<int>("Animations/Actor/Standing/Down");
            animationStandingDown.AddFrame(2, 2.0f);
            var animationStandingDownLeft = core.Animations.Anims.Create<int>("Animations/Actor/Standing/DownLeft");
            animationStandingDownLeft.AddFrame(3, 2.0f);
            var animationStandingLeft = core.Animations.Anims.Create<int>("Animations/Actor/Standing/Left");
            animationStandingLeft.AddFrame(4, 2.0f);
            var animationStandingLeftUp = core.Animations.Anims.Create<int>("Animations/Actor/Standing/LeftUp");
            animationStandingLeftUp.AddFrame(5, 2.0f);
            var animationStandingUp = core.Animations.Anims.Create<int>("Animations/Actor/Standing/Up");
            animationStandingUp.AddFrame(6, 2.0f);
            var animationStandingUpRight = core.Animations.Anims.Create<int>("Animations/Actor/Standing/UpRight");
            animationStandingUpRight.AddFrame(7, 2.0f);

            var animationWalkingRight = core.Animations.Anims.Create<int>("Animations/Actor/Walking/Right");
            animationWalkingRight.AddFrame(0, 1.0f);
            animationWalkingRight.AddFrame(8, 1.0f);
            animationWalkingRight.AddFrame(16, 1.0f);
            animationWalkingRight.AddFrame(24, 1.0f);
            animationWalkingRight.AddFrame(32, 1.0f);
            var animationWalkingRightDown = core.Animations.Anims.Create<int>("Animations/Actor/Walking/RightDown");
            animationWalkingRightDown.AddFrame(1, 1.0f);
            animationWalkingRightDown.AddFrame(9, 1.0f);
            animationWalkingRightDown.AddFrame(17, 1.0f);
            animationWalkingRightDown.AddFrame(25, 1.0f);
            animationWalkingRightDown.AddFrame(33, 1.0f);
            var animationWalkingDown = core.Animations.Anims.Create<int>("Animations/Actor/Walking/Down");
            animationWalkingDown.AddFrame(2, 1.0f);
            animationWalkingDown.AddFrame(10, 1.0f);
            animationWalkingDown.AddFrame(18, 1.0f);
            animationWalkingDown.AddFrame(26, 1.0f);
            animationWalkingDown.AddFrame(34, 1.0f);
            var animationWalkingDownLeft = core.Animations.Anims.Create<int>("Animations/Actor/Walking/DownLeft");
            animationWalkingDownLeft.AddFrame(3, 1.0f);
            animationWalkingDownLeft.AddFrame(11, 1.0f);
            animationWalkingDownLeft.AddFrame(19, 1.0f);
            animationWalkingDownLeft.AddFrame(27, 1.0f);
            animationWalkingDownLeft.AddFrame(35, 1.0f);
            var animationWalkingLeft = core.Animations.Anims.Create<int>("Animations/Actor/Walking/Left");
            animationWalkingLeft.AddFrame(4, 1.0f);
            animationWalkingLeft.AddFrame(12, 1.0f);
            animationWalkingLeft.AddFrame(20, 1.0f);
            animationWalkingLeft.AddFrame(28, 1.0f);
            animationWalkingLeft.AddFrame(36, 1.0f);
            var animationWalkingLeftUp = core.Animations.Anims.Create<int>("Animations/Actor/Walking/LeftUp");
            animationWalkingLeftUp.AddFrame(5, 1.0f);
            animationWalkingLeftUp.AddFrame(13, 1.0f);
            animationWalkingLeftUp.AddFrame(21, 1.0f);
            animationWalkingLeftUp.AddFrame(29, 1.0f);
            animationWalkingLeftUp.AddFrame(37, 1.0f);
            var animationWalkingUp = core.Animations.Anims.Create<int>("Animations/Actor/Walking/Up");
            animationWalkingUp.AddFrame(6, 1.0f);
            animationWalkingUp.AddFrame(14, 1.0f);
            animationWalkingUp.AddFrame(22, 1.0f);
            animationWalkingUp.AddFrame(30, 1.0f);
            animationWalkingUp.AddFrame(38, 1.0f);
            var animationWalkingUpRight = core.Animations.Anims.Create<int>("Animations/Actor/Walking/UpRight");
            animationWalkingUpRight.AddFrame(7, 1.0f);
            animationWalkingUpRight.AddFrame(15, 1.0f);
            animationWalkingUpRight.AddFrame(23, 1.0f);
            animationWalkingUpRight.AddFrame(31, 1.0f);
            animationWalkingUpRight.AddFrame(39, 1.0f);
        }

        private static void OnCollision(IEntity thisEntity, IEntity otherEntity, Vector2 projection)
        {
            thisEntity.RaiseEvent(new CollisionEvent(otherEntity));

            var body = otherEntity.Components.OfType<IBody>().FirstOrDefault();

            var type = body.Tag;

            switch (type)
            {
                case "Static":
                    DynamicHelper.ResolveVsStatic(thisEntity, otherEntity, projection);
                    return;
                default:
                    break;
            }  
        }

        public static IEntity CreateActor(ICore core, Vector2 pos)
        {
            var actor = core.Entities.Create();
            actor.Add(new Animator(10.0f, true));
            //actor.Add(new CollisionDebug(Core.Rendering.CreateSprite(spriteAtlas.Id)));
            actor.Add(core.Rendering.CreateSprite("Atlases/Sprites/Arrow"));
            actor.Add(Position.Create(pos));
            actor.Add(Thrust.Create(0, 0));
            actor.Add(Velocity.Create(0, 0));
            actor.Add(Direction.Create(1, 0));
            actor.Add(new InventoryComponent(new Bag[] { new Bag("Backpack") }));
            actor.Add(new EquipmentComponent(new Slot[] { new Slot("Torso"), new Slot("Hands") }));
            actor.Add(AxisAlignedBoxShape.Create(0, 0, 32, 32));
            actor.Add(new Motion());
            actor.Add(Body.Create(1.0f, 0.0f, "Dynamic", (e,c) => OnCollision(actor,e,c)));

            return actor;
        }

        public static StateMachine CreateAttackingFSM(IEntity entity)
        {
            var stateMachine = entity.AddFSM("Attacking");

            stateMachine.AddState(new ShootingState("Shooting"));
            stateMachine.AddState(new States.Attacking.IdleState("Idle"));
            stateMachine.AddState(new CooldownState("Cooldown"));

            return stateMachine;
        }

        public static StateMachine CreateRotationFSM(IEntity entity)
        {
            var stateMachine = entity.AddFSM("Rotation");

            stateMachine.AddState(new States.Rotation.IdleState("Idle"));
            stateMachine.AddState(new RotatingState("Rotating", "Animations/Actor"));

            return stateMachine;
        }

        public static StateMachine CreateMovementFSM(IEntity entity)
        {
            var stateMachine = entity.AddFSM("Movement");

            stateMachine.AddState(new StandingState("Standing", "Animations/Actor"));
            stateMachine.AddState(new WalkingState("Walking", "Animations/Actor"));

            return stateMachine;
        }
    }
}

using OpenBreed.Components.Physics;
using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Components.Common;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Helpers;
using OpenBreed.Physics.Generic.Helpers;
using OpenBreed.Systems.Rendering.Commands;
using OpenBreed.Sandbox.Entities.Actor.States.Attacking;
using OpenBreed.Sandbox.Entities.Actor.States.Movement;
using OpenBreed.Sandbox.Entities.Actor.States.Rotation;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.CodeDom;
using System.Linq;
using OpenBreed.Systems.Animation.Commands;
using OpenBreed.Systems.Control.Events;
using OpenBreed.Ecsw.Entities;
using OpenBreed.Fsm;
using OpenBreed.Ecsw;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorRotationHelper
    {
        #region Public Methods

        public static void CreateFsm(ICore core)
        {
            var stateMachine = core.GetManager<IFsmMan>().Create<RotationState, RotationImpulse>("Actor.Rotation");

            stateMachine.AddState(new States.Rotation.IdleState());
            stateMachine.AddState(new States.Rotation.RotatingState());

            stateMachine.AddTransition(RotationState.Rotating, RotationImpulse.Stop, RotationState.Idle);
            stateMachine.AddTransition(RotationState.Idle, RotationImpulse.Rotate, RotationState.Rotating);

            //stateMachine.AddOnEnterState(RotationState.Idle, RotationImpulse.Stop, OnRotationEnterIdleWithStop);
            //stateMachine.AddOnEnterState(RotationState.Rotating, RotationImpulse.Rotate, OnRotationEnterRotatingWithRotate);
            //stateMachine.AddOnLeaveState(RotationState.Idle, RotationImpulse.Rotate, OnRotationLeaveIdleWithRotate);
            //stateMachine.AddOnLeaveState(RotationState.Rotating, RotationImpulse.Stop, OnRotationLeaveRotatingWithStop);
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnRotationLeaveRotatingWithStop(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);
            Console.WriteLine("Rotation (!Rotating -> Stop)");
        }

        private static void OnRotationEnterRotatingWithRotate(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);
            var direction = entity.Get<AngularPositionComponent>();
            var movement = entity.Get<MotionComponent>();
            entity.Get<ThrustComponent>().Value = direction.GetDirection() * movement.Acceleration;

            var animDirName = AnimHelper.ToDirectionName(direction.GetDirection());
            var className = entity.Get<ClassComponent>().Name;
            var movementFsm = entity.Core.GetManager<IFsmMan>().GetByName("Actor.Movement");
            var movementStateName = movementFsm.GetCurrentStateName(entity);
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{"Animations"}/{className}/{movementStateName}/{animDirName}", 0));

            var currentStateNames = entity.Core.GetManager<IFsmMan>().GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Core.Commands.Post(new SetStateCommand(entity.Id, fsmId, (int)RotationImpulse.Stop));
        }

        private static void OnRotationEnterIdleWithStop(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);

            // Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            var currentStateNames = entity.Core.GetManager<IFsmMan>().GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        private static void OnRotationLeaveIdleWithRotate(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);
            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }


        private static void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            var entity = sender as Entity;

            if (e.Direction != Vector2.Zero)
            {
                var angularPos = entity.Get<AngularPositionComponent>();

                if (angularPos.GetDirection() != e.Direction)
                {
                    var aPos3 = new Vector3(angularPos.GetDirection());
                    var dPos3 = new Vector3(e.Direction);
                    var newVec = Vector3Extension.RotateTowards(aPos3, dPos3, 0.1f, 1.0f);
                    var angularThrust = entity.Get<AngularVelocityComponent>();
                    angularThrust.SetDirection(new Vector2(newVec.X, newVec.Y));


                    //angularPos.SetDirection(e.Direction);
                    var fsmId = entity.Core.GetManager<IFsmMan>().GetByName("Actor.Rotation").Id;
                    entity.Core.Commands.Post(new SetStateCommand(entity.Id, fsmId, (int)RotationImpulse.Rotate));
                }
            }
        }


        #endregion Private Methods
    }
}
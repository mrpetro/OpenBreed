using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Sandbox.Entities.Actor.States.Movement;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Wecs;
using OpenBreed.Core.Managers;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorMovementHelper
    {
        #region Public Methods

        public static void CreateFsm(ICore core)
        {
            var fsmMan = core.GetManager<IFsmMan>();
            var commandsMan = core.GetManager<ICommandsMan>();

            var stateMachine = fsmMan.Create<MovementState, MovementImpulse>("Actor.Movement");

            stateMachine.AddState(new StandingState(fsmMan, commandsMan));
            stateMachine.AddState(new WalkingState(fsmMan, commandsMan));

            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Stop, MovementState.Standing);
            stateMachine.AddTransition(MovementState.Standing, MovementImpulse.Walk, MovementState.Walking);
            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Walk, MovementState.Walking);
        }
    }
}
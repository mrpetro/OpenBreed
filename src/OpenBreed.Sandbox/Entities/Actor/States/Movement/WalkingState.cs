﻿
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Systems.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Components.Physics;
using OpenBreed.Systems.Physics.Events;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public class WalkingState : IState<MovementState, MovementImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;

        #endregion Private Fields

        #region Public Constructors

        public WalkingState()
        {
            this.animPrefix = "Animations";
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)MovementState.Walking;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            var direction = entity.Get<AngularPositionComponent>();
            var movement = entity.Get<MotionComponent>();
            var control = entity.Get<WalkingControl>();
            entity.Get<ThrustComponent>().Value = control.Direction * movement.Acceleration;

            var animDirPostfix = AnimHelper.ToDirectionName(direction.GetDirection());

            var stateName = entity.Core.StateMachines.GetStateName(FsmId, Id);
            var className = entity.Get<ClassComponent>().Name;
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{animPrefix}/{className}/{stateName}/{animDirPostfix}", 0));

            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
            entity.Subscribe<DirectionChangedEventArgs>(OnDirectionChanged);
        }
     
        public void LeaveState(Entity entity)
        {
            entity.Subscribe<DirectionChangedEventArgs>(OnDirectionChanged);
            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnDirectionChanged(object sender, DirectionChangedEventArgs args)
        {
            var entity = sender as Entity;

            var direction = entity.Get<AngularPositionComponent>();
            var movement = entity.Get<MotionComponent>();
            //entity.Get<VelocityComponent>().Value = Vector2.Zero;
            //entity.Get<ThrustComponent>().Value = direction.GetDirection() * movement.Acceleration;
            var animDirName = AnimHelper.ToDirectionName(direction.GetDirection());
            var className = entity.Get<ClassComponent>().Name;
            var movementFsm = entity.Core.StateMachines.GetByName("Actor.Movement");
            var movementStateName = movementFsm.GetCurrentStateName(entity);
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{"Animations"}/{className}/{movementStateName}/{animDirName}", 0));

            //throw new NotImplementedException();
        }

        private void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            var entity = sender as Entity;

            if (e.Direction != Vector2.Zero)
            {
                entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)MovementImpulse.Walk));
                var angularThrust = entity.Get<AngularVelocityComponent>();
                angularThrust.SetDirection(new Vector2(e.Direction.X, e.Direction.Y));
            }
            else
                entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)MovementImpulse.Stop));


        }

        #endregion Private Methods
    }
}
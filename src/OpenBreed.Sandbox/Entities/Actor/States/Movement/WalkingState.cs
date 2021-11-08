
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Animation.Interface;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public class WalkingState : IState<MovementState, MovementImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;
        private readonly IFsmMan fsmMan;
        private readonly ICommandsMan commandsMan;
        private readonly IClipMan clipMan;

        #endregion Private Fields

        #region Public Constructors

        public WalkingState(IFsmMan fsmMan, ICommandsMan commandsMan, IClipMan clipMan)
        {
            this.animPrefix = "Animations";
            this.fsmMan = fsmMan;
            this.commandsMan = commandsMan;
            this.clipMan = clipMan;
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
            var control = entity.Get<WalkingControlComponent>();
            entity.Get<ThrustComponent>().Value = control.Direction * movement.Acceleration;

            var animDirPostfix = AnimHelper.ToDirectionName(direction.Value);
            var stateName = fsmMan.GetStateName(FsmId, Id);
            var className = entity.Get<ClassComponent>().Name;
            var clip = clipMan.GetByName($"{animPrefix}/{className}/{stateName}/{animDirPostfix}");
            var currentStateNames = fsmMan.GetStateNames(entity);

            entity.PlayAnimation(0, clip.Id);
            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
            entity.Subscribe<DirectionChangedEventArgs>(OnDirectionChanged);

        }

    
        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<DirectionChangedEventArgs>(OnDirectionChanged);
            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnDirectionChanged(object sender, DirectionChangedEventArgs args)
        {
            var entity = sender as Entity;

            var direction = entity.Get<AngularPositionComponent>();
            var movement = entity.Get<MotionComponent>();
            var animDirName = AnimHelper.ToDirectionName(direction.Value);
            var className = entity.Get<ClassComponent>().Name;
            var movementFsm = fsmMan.GetByName("Actor.Movement");
            var movementStateName = movementFsm.GetCurrentStateName(entity);

            var clip = clipMan.GetByName($"{"Animations"}/{className}/{movementStateName}/{animDirName}");

            entity.PlayAnimation(0, clip.Id);
        }

        private void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            var entity = sender as Entity;

            if (e.Direction != Vector2.Zero)
            {
                entity.SetState(FsmId, (int)MovementImpulse.Walk);

                var angularThrust = entity.Get<AngularVelocityComponent>();
                angularThrust.Value = new Vector2(e.Direction.X, e.Direction.Y);
            }
            else
            {
                entity.SetState(FsmId, (int)MovementImpulse.Stop);
            }
        }

        #endregion Private Methods
    }
}
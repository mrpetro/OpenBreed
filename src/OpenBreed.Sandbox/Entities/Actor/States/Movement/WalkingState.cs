
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Core.Commands;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public class WalkingState : IState<MovementState, MovementImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;

        #endregion Private Fields

        #region Public Constructors

        public WalkingState( string animPrefix)
        {
            this.animPrefix = animPrefix;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)MovementState.Walking;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            var direction = entity.GetComponent<DirectionComponent>();
            var movement = entity.GetComponent<MotionComponent>();
            entity.GetComponent<ThrustComponent>().Value = direction.Value * movement.Acceleration;

            var animDirPostfix = AnimHelper.ToDirectionName(direction.Value);

            var stateName = entity.Core.StateMachines.GetStateName(FsmId, Id);
            entity.PostCommand(new PlayAnimCommand(entity.Id, $"{animPrefix}/{stateName}/{animDirPostfix}", 0));

            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.PostCommand(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }
     
        public void LeaveState(IEntity entity)
        {
            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            var entity = sender as IEntity;

            if (e.Direction != Vector2.Zero)
                entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)MovementImpulse.Walk));
            else
                entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)MovementImpulse.Stop));
        }

        #endregion Private Methods
    }
}
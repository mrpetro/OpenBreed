
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Core.Commands;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public class StandingState : IStateEx<MovementState, MovementImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;

        #endregion Private Fields

        #region Public Constructors

        public StandingState(string animPrefix)
        {
            this.animPrefix = animPrefix;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)MovementState.Standing;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            var direction = entity.GetComponent<DirectionComponent>().Value;

            var animDirName = AnimHelper.ToDirectionName(direction);

            var thrust = entity.GetComponent<ThrustComponent>();

            thrust.Value = Vector2.Zero;

            var stateName = entity.Core.StateMachines.GetStateName(FsmId, Id);
            entity.PostCommand(new PlayAnimCommand(entity.Id,  $"{animPrefix}/{stateName}/{animDirName}", 0));

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

        private void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs eventArgs)
        {
            var entity = sender as IEntity;

            if (eventArgs.Direction != Vector2.Zero)
                entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)MovementImpulse.Walk));
        }

        #endregion Private Methods
    }
}
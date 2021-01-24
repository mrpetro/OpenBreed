using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public class StandingState : IState<MovementState, MovementImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;

        #endregion Private Fields

        #region Public Constructors

        public StandingState()
        {
            this.animPrefix = "Animations";
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)MovementState.Standing;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            var direction = entity.Get<AngularPositionComponent>().GetDirection();

            var animDirName = AnimHelper.ToDirectionName(direction);
            var className = entity.Get<ClassComponent>().Name;

            var thrust = entity.Get<ThrustComponent>();

            thrust.Value = Vector2.Zero;

            var stateName = entity.Core.GetManager<IFsmMan>().GetStateName(FsmId, Id);
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{animPrefix}/{className}/{stateName}/{animDirName}", 0));

            var currentStateNames = entity.Core.GetManager<IFsmMan>().GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs eventArgs)
        {
            var entity = sender as Entity;

            if (eventArgs.Direction != Vector2.Zero)
            {
                entity.Core.Commands.Post(new SetEntityStateCommand(entity.Id, FsmId, (int)MovementImpulse.Walk));

                var angularThrust = entity.Get<AngularVelocityComponent>();
                angularThrust.SetDirection(new Vector2(eventArgs.Direction.X, eventArgs.Direction.Y));
            }
        }

        #endregion Private Methods
    }
}
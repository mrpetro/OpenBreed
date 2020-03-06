
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Control.Events;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Core.Commands;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public class StandingState : IState
    {
        #region Private Fields

        private readonly string animPrefix;
        private ThrustComponent thrust;

        #endregion Private Fields

        #region Public Constructors

        public StandingState(string id, string animPrefix)
        {
            Name = id;
            this.animPrefix = animPrefix;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods



        public void EnterState()
        {
            var direction = Entity.GetComponent<Direction>().Value;

            var animDirName = AnimHelper.ToDirectionName(direction);

            thrust.Value = Vector2.Zero;

            Entity.PostCommand(new PlayAnimCommand(Entity.Id,  $"{animPrefix}/{Name}/{animDirName}"));
            Entity.PostCommand(new TextSetCommand(Entity.Id, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            Entity.Subscribe(ControlEventTypes.CONTROL_DIRECTION_CHANGED, OnControlDirectionChanged);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            thrust = entity.GetComponent<ThrustComponent>();
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(ControlEventTypes.CONTROL_DIRECTION_CHANGED, OnControlDirectionChanged);
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Walk":
                    {
                        return "Walking";
                    }
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void HandleControlDirectionChangedEvent(ControlDirectionChangedEvent systemEvent)
        {
            if (systemEvent.Direction != Vector2.Zero)
                Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "Movement", "Walk"));
        }

        private void OnControlDirectionChanged(object sender, EventArgs eventArgs)
        {
            HandleControlDirectionChangedEvent((ControlDirectionChangedEvent)eventArgs);
        }

        #endregion Private Methods
    }
}
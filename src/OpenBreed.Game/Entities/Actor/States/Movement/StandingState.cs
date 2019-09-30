using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Game.Helpers;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Game.Entities.Actor.States.Movement
{
    public class StandingState : IState
    {
        #region Private Fields

        private readonly string animPrefix;
        private IThrust thrust;

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
            var direction = Entity.Components.OfType<Direction>().First().Value;

            var animDirName = AnimHelper.ToDirectionName(direction);

            thrust.Value = Vector2.Zero;

            Entity.PostMsg(new PlayAnimMsg(Entity,  $"{animPrefix}/{Name}/{animDirName}"));
            Entity.PostMsg(new TextSetMsg(Entity, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            Entity.Subscribe(ControlDirectionChangedEvent.TYPE, OnControlDirectionChanged);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            thrust = entity.Components.OfType<IThrust>().First();
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(ControlDirectionChangedEvent.TYPE, OnControlDirectionChanged);
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
                Entity.PostMsg(new StateChangeMsg(Entity, "Movement", "Walk"));
        }

        private void OnControlDirectionChanged(object sender, IEvent e)
        {
            HandleControlDirectionChangedEvent((ControlDirectionChangedEvent)e);
        }

        #endregion Private Methods
    }
}
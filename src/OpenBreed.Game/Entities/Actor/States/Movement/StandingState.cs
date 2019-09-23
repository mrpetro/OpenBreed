using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenTK;
using System.Linq;

namespace OpenBreed.Game.Entities.Actor.States.Movement
{
    public class StandingState : IState
    {
        #region Private Fields

        private readonly string animationId;
        private readonly Vector2 facingDirection;
        private IThrust thrust;
        private Animator<int> spriteAnimation;

        #endregion Private Fields

        #region Public Constructors

        public StandingState(string id, string animationId, Vector2 facingDirection)
        {
            Id = id;
            this.animationId = animationId;
            this.facingDirection = facingDirection;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Id { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(object[] arguments)
        {
            thrust.Value = Vector2.Zero;

            Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostMsg(new TextSetMsg(Entity, "Hero - Standing"));

            Entity.Subscribe(ControlDirectionChangedEvent.TYPE, OnControlDirectionChanged);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            thrust = entity.Components.OfType<IThrust>().First();
            spriteAnimation = entity.Components.OfType<Animator<int>>().First();
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
                        var walkDirection = (Vector2)arguments[0];

                        if (walkDirection.X == 1 && walkDirection.Y == 0)
                            return "Walking_Right";
                        else if (walkDirection.X == 1 && walkDirection.Y == -1)
                            return "Walking_Right_Down";
                        else if (walkDirection.X == 0 && walkDirection.Y == -1)
                            return "Walking_Down";
                        else if (walkDirection.X == -1 && walkDirection.Y == -1)
                            return "Walking_Down_Left";
                        else if (walkDirection.X == -1 && walkDirection.Y == 0)
                            return "Walking_Left";
                        else if (walkDirection.X == -1 && walkDirection.Y == 1)
                            return "Walking_Left_Up";
                        else if (walkDirection.X == 0 && walkDirection.Y == 1)
                            return "Walking_Up";
                        else if (walkDirection.X == 1 && walkDirection.Y == 1)
                            return "Walking_Up_Right";
                        break;
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
                Entity.PostMsg(new StateChangeMsg(Entity, "Movement", "Walk", systemEvent.Direction));
            else
                Entity.PostMsg(new StateChangeMsg(Entity, "Movement", "Stop"));
        }

        private void OnControlDirectionChanged(object sender, IEvent e)
        {
            HandleControlDirectionChangedEvent((ControlDirectionChangedEvent)e);
        }

        #endregion Private Methods
    }
}
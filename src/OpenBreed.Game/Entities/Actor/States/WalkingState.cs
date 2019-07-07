using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Systems.Animation.Components;
using OpenBreed.Core.Systems.Animation.Events;
using OpenBreed.Core.Systems.Animation.Messages;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control.Events;
using OpenBreed.Core.Systems.Movement.Components;
using OpenTK;
using System.Linq;

namespace OpenBreed.Game.Entities.Actor.States
{
    public class WalkingState : IState
    {
        #region Private Fields

        private readonly string animationId;
        private readonly Vector2 walkDirection;
        private IThrust thrust;
        private Motion creatureMovement;
        private Animator<int> spriteAnimation;
        private ISprite sprite;
        private IDirection direction;

        #endregion Private Fields

        #region Public Constructors

        public WalkingState(string id, string animationId, Vector2 walkDirection)
        {
            Id = id;
            this.animationId = animationId;
            this.walkDirection = walkDirection;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Id { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            thrust.Value = walkDirection * creatureMovement.Speed;

            Entity.PostMessage(new PlayAnimMsg(animationId));
            Entity.PostMessage(new SetTextMsg("Hero - Walking"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            thrust = entity.Components.OfType<IThrust>().First();
            direction = entity.Components.OfType<IDirection>().First();
            creatureMovement = entity.Components.OfType<Motion>().First();
            sprite = entity.Components.OfType<ISprite>().First();
            spriteAnimation = entity.Components.OfType<Animator<int>>().First();

            entity.HandleSystemEvent = HandleSystemEvent;
        }

        public void LeaveState()
        {
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Stop":
                    {
                        if (walkDirection.X == 1 && walkDirection.Y == 0)
                            return "Standing_Right";
                        else if (walkDirection.X == 1 && walkDirection.Y == -1)
                            return "Standing_Right_Down";
                        else if (walkDirection.X == 0 && walkDirection.Y == -1)
                            return "Standing_Down";
                        else if (walkDirection.X == -1 && walkDirection.Y == -1)
                            return "Standing_Down_Left";
                        else if (walkDirection.X == -1 && walkDirection.Y == 0)
                            return "Standing_Left";
                        else if (walkDirection.X == -1 && walkDirection.Y == 1)
                            return "Standing_Left_Up";
                        else if (walkDirection.X == 0 && walkDirection.Y == 1)
                            return "Standing_Up";
                        else if (walkDirection.X == 1 && walkDirection.Y == 1)
                            return "Standing_Up_Right";
                        break;
                    }
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

        private void HandleSystemEvent(IWorldSystem system, ISystemEvent systemEvent)
        {
            switch (systemEvent.Type)
            {
                case FrameChangedEvent<int>.TYPE:
                    HandleFrameChangeEvent(system, (FrameChangedEvent<int>)systemEvent);
                    break;
                case ControlDirectionChangedEvent.TYPE:
                    HandleControlDirectionChangedEvent(system, (ControlDirectionChangedEvent)systemEvent);
                    break;
                default:
                    break;
            }
        }

        private void HandleFrameChangeEvent(IWorldSystem system, FrameChangedEvent<int> systemEvent)
        {
            sprite.ImageId = systemEvent.Frame;
        }

        private void HandleControlDirectionChangedEvent(IWorldSystem system, ControlDirectionChangedEvent systemEvent)
        {
            if (systemEvent.Direction != Vector2.Zero)
                Entity.PostMessage(new StateChangeMsg("Walk", systemEvent.Direction));
            else
                Entity.PostMessage(new StateChangeMsg("Stop"));
        }

        #endregion Private Methods
    }
}
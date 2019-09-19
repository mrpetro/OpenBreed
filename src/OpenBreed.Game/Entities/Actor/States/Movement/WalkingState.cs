using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using System.Linq;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Common.Helpers;

namespace OpenBreed.Game.Entities.Actor.States.Movement
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
            thrust.Value = walkDirection * creatureMovement.Acceleration;

            Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostMsg(new TextSetMsg(Entity, "Hero - Walking"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            thrust = entity.Components.OfType<IThrust>().First();
            creatureMovement = entity.Components.OfType<Motion>().First();
            sprite = entity.Components.OfType<ISprite>().First();
            spriteAnimation = entity.Components.OfType<Animator<int>>().First();


            Entity.Subscribe(AnimChangedEvent<int>.TYPE, OnFrameChanged);
            Entity.Subscribe(ControlDirectionChangedEvent.TYPE, OnControlDirectionChanged);
        }

        private void OnFrameChanged(object sender, IEvent e)
        {
            HandleFrameChangeEvent((AnimChangedEvent<int>)e);
        }

        private void OnControlDirectionChanged(object sender, IEvent e)
        {
            HandleControlDirectionChangedEvent((ControlDirectionChangedEvent)e);
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

        private void HandleFrameChangeEvent(AnimChangedEvent<int> systemEvent)
        {
            sprite.ImageId = systemEvent.Frame;
        }

        private void HandleControlDirectionChangedEvent(ControlDirectionChangedEvent systemEvent)
        {
            if (systemEvent.Direction != Vector2.Zero)
                Entity.PostMsg(new StateChangeMsg(Entity, "Movement", "Walk", systemEvent.Direction));
            else
                Entity.PostMsg(new StateChangeMsg(Entity, "Movement", "Stop"));
        }

        #endregion Private Methods
    }
}
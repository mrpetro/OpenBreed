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

namespace OpenBreed.Game.Components.States
{
    public class ClosingState : IState
    {
        #region Private Fields

        private readonly string animationId;
        private Animator<int> spriteAnimation;
        private ISprite sprite;

        #endregion Private Fields

        #region Public Constructors

        public ClosingState(string id, string animationId)
        {
            Id = id;
            this.animationId = animationId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Id { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            Entity.PostMessage(new PlayAnimMsg(animationId));
            Entity.PostMessage(new SetTextMsg("Door - Closing"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
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
                        break;
                    }
                case "Walk":
                    {
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
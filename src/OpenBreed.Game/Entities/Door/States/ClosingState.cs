using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using System.Linq;
using OpenBreed.Core.Common.Systems;

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
            Entity.Core.MessageBus.PostMsg(this, new PlayAnimMsg(Entity, animationId));
            Entity.Core.MessageBus.PostMsg(this, new SetTextMsg(Entity, "Door - Closing"));
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
                Entity.PostMsg(new StateChangeMsg(Entity, "Walk", systemEvent.Direction));
            else
                Entity.PostMsg(new StateChangeMsg(Entity, "Stop"));
        }

        #endregion Private Methods
    }
}
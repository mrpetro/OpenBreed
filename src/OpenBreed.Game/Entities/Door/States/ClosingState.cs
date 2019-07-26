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
using OpenBreed.Core.Common.Helpers;

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
            Entity.Core.MessageBus.Enqueue(this, new PlayAnimMsg(Entity, animationId));
            Entity.Core.MessageBus.Enqueue(this, new SetTextMsg(Entity, "Door - Closing"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            sprite = entity.Components.OfType<ISprite>().First();
            spriteAnimation = entity.Components.OfType<Animator<int>>().First();

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

    }
}
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using System.Linq;

namespace OpenBreed.Game.Components.States
{
    public class OpeningState : IState
    {
        #region Private Fields

        private readonly string animationId;
        private Animator<int> animator;
        private ISprite sprite;

        #endregion Private Fields

        #region Public Constructors

        public OpeningState(string id, string animationId)
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
            Entity.PostMsg(new SpriteOnMsg(Entity));
            Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostMsg(new TextSetMsg(Entity, "Door - Opening"));

            Entity.Subscribe(AnimChangedEvent<int>.TYPE, OnAnimChanged);
            Entity.Subscribe(AnimStoppedEvent<int>.TYPE, OnAnimStopped);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            animator = entity.Components.OfType<Animator<int>>().First();
            sprite = entity.Components.OfType<ISprite>().First();
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(AnimChangedEvent<int>.TYPE, OnAnimChanged);
            Entity.Unsubscribe(AnimStoppedEvent<int>.TYPE, OnAnimStopped);
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Opened":
                    return "Opened";
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimChanged(object sender, IEvent e)
        {
            HandleAnimChangeEvent((AnimChangedEvent<int>)e);
        }

        private void OnAnimStopped(object sender, IEvent e)
        {
            HandleAnimStoppedEvent((AnimStoppedEvent<int>)e);
        }

        private void HandleAnimChangeEvent(AnimChangedEvent<int> e)
        {
            sprite.ImageId = e.Frame;
        }

        private void HandleAnimStoppedEvent(AnimStoppedEvent<int> e)
        {
            Entity.PostMsg(new StateChangeMsg(Entity, "Opened"));
        }

        #endregion Private Methods
    }
}
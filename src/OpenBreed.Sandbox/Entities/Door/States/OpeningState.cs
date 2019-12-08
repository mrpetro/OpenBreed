using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using System.Linq;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpeningState : IState
    {
        #region Private Fields

        private readonly string animationId;

        #endregion Private Fields

        #region Public Constructors

        public OpeningState(string id, string animationId)
        {
            Name = id;
            this.animationId = animationId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            Entity.PostMsg(new SpriteOnMsg(Entity.Id));
            Entity.PostMsg(new PlayAnimMsg(Entity.Id, animationId));
            Entity.PostMsg(new TextSetMsg(Entity.Id, "Door - Opening"));

            Entity.Subscribe(AnimChangedEvent.TYPE, OnAnimChanged);
            Entity.Subscribe(AnimStoppedEvent.TYPE, OnAnimStopped);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(AnimChangedEvent.TYPE, OnAnimChanged);
            Entity.Unsubscribe(AnimStoppedEvent.TYPE, OnAnimStopped);
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
            HandleAnimChangeEvent((AnimChangedEvent)e);
        }

        private void OnAnimStopped(object sender, IEvent e)
        {
            HandleAnimStoppedEvent((AnimStoppedEvent)e);
        }

        private void HandleAnimChangeEvent(AnimChangedEvent e)
        {
            Entity.PostMsg(new SpriteSetMsg(Entity.Id, (int)e.Frame));
        }

        private void HandleAnimStoppedEvent(AnimStoppedEvent e)
        {
            Entity.PostMsg(new StateChangeMsg(Entity.Id, "Functioning", "Opened"));
        }

        #endregion Private Methods
    }
}
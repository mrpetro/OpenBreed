
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using System;
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
            Entity.PostCommand(new SpriteOnCommand(Entity.Id));
            Entity.PostCommand(new PlayAnimCommand(Entity.Id, animationId));
            Entity.PostCommand(new TextSetCommand(Entity.Id, "Door - Opening"));

            Entity.Subscribe(AnimationEventTypes.ANIMATION_CHANGED, OnAnimChanged);
            Entity.Subscribe(AnimationEventTypes.ANIMATION_STOPPED, OnAnimStopped);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(AnimationEventTypes.ANIMATION_CHANGED, OnAnimChanged);
            Entity.Unsubscribe(AnimationEventTypes.ANIMATION_STOPPED, OnAnimStopped);
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

        private void OnAnimChanged(object sender, EventArgs eventArgs)
        {
            HandleAnimChangeEvent((AnimChangedEventArgs)eventArgs);
        }

        private void OnAnimStopped(object sender, EventArgs eventArgs)
        {
            HandleAnimStoppedEvent((AnimStoppedEventArgs)eventArgs);
        }

        private void HandleAnimChangeEvent(AnimChangedEventArgs e)
        {
            Entity.PostCommand(new SpriteSetCommand(Entity.Id, (int)e.Frame));
        }

        private void HandleAnimStoppedEvent(AnimStoppedEventArgs e)
        {
            Entity.PostCommand(new StateChangeCommand(Entity.Id, "Functioning", "Opened"));
        }

        #endregion Private Methods
    }
}

using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using System;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Sandbox.Entities.Door.States;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpeningState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string animationId;

        #endregion Private Fields

        #region Public Constructors

        public OpeningState(string animationId)
        {
            this.animationId = animationId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public FunctioningState Id => FunctioningState.Opening;

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            Entity.PostCommand(new SpriteOnCommand(Entity.Id));
            Entity.PostCommand(new PlayAnimCommand(Entity.Id, animationId));
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, "Door - Opening"));

            Entity.Subscribe<AnimChangedEventArgs>(OnAnimChanged);
            Entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe<AnimChangedEventArgs>(OnAnimChanged);
            Entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public FunctioningState Process(FunctioningImpulse impulse, object[] arguments)
        {
            switch (impulse)
            {
                case FunctioningImpulse.StopOpening:
                    return FunctioningState.Opened;
                default:
                    break;
            }

            return Id;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimChanged(object sender, AnimChangedEventArgs e)
        {
            Entity.PostCommand(new SpriteSetCommand(Entity.Id, (int)e.Frame));
        }

        private void OnAnimStopped(object sender, AnimStoppedEventArgs e)
        {
            Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "FunctioningState", "StopOpening"));
        }

        #endregion Private Methods
    }
}
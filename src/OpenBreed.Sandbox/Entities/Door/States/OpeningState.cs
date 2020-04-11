
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
    public class OpeningState : IStateEx<FunctioningState, FunctioningImpulse>
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

        public int Id => (int)(ValueType)FunctioningState.Opening;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            entity.PostCommand(new SpriteOnCommand(entity.Id));
            entity.PostCommand(new PlayAnimCommand(entity.Id, animationId, 0));
            entity.PostCommand(new TextSetCommand(entity.Id, 0, "Door - Opening"));

            entity.Subscribe<AnimChangedEventArgs>(OnAnimChanged);
            entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public void LeaveState(IEntity entity)
        {
            entity.Unsubscribe<AnimChangedEventArgs>(OnAnimChanged);
            entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimChanged(object sender, AnimChangedEventArgs e)
        {
            var entity = sender as IEntity;

            entity.PostCommand(new SpriteSetCommand(entity.Id, (int)e.Frame));
        }

        private void OnAnimStopped(object sender, AnimStoppedEventArgs e)
        {
            var entity = sender as IEntity;
            entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)FunctioningImpulse.StopOpening));
        }

        #endregion Private Methods
    }
}
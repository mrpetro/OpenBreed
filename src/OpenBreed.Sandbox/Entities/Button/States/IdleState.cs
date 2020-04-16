﻿
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

namespace OpenBreed.Sandbox.Entities.Button.States
{
    public class IdleState : IStateEx<ButtonState, ButtonImpulse>
    {
        public  const string NAME = "Idle";

        #region Private Fields

        private readonly string animationId;

        #endregion Private Fields

        #region Public Constructors

        public IdleState()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)ButtonState.Idle;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            entity.PostCommand(new SpriteOnCommand(entity.Id));
            entity.PostCommand(new PlayAnimCommand(entity.Id, animationId, 0));
            entity.PostCommand(new TextSetCommand(entity.Id, 0, "Door - Opening"));

            entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public void LeaveState(IEntity entity)
        {
            entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimStopped(object sender, AnimStoppedEventArgs eventArgs)
        {
            var entity = sender as IEntity;
            entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)ButtonImpulse.Press));

        }

        #endregion Private Methods
    }
}
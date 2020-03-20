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

        private void OnAnimChanged(object sender, AnimChangedEventArgs e)
        {
            Entity.PostCommand(new SpriteSetCommand(Entity.Id, (int)e.Frame));
        }

        private void OnAnimStopped(object sender, AnimStoppedEventArgs e)
        {
            Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "Functioning", "Opened"));
        }

        #endregion Private Methods
    }
}
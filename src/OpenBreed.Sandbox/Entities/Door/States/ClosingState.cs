using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using System.Linq;
using OpenBreed.Core.Common.Systems;

using OpenBreed.Core.Modules.Physics.Commands;
using OpenBreed.Sandbox.Entities.Door.States;
using System;

namespace OpenBreed.Sandbox.Components.States
{
    public class ClosingState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string animationId;

        #endregion Private Fields

        #region Public Constructors

        public ClosingState(string animationId)
        {
            this.animationId = animationId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public FunctioningState Id => FunctioningState.Closing;

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            Entity.PostCommand(new SpriteOnCommand(Entity.Id));
            Entity.PostCommand(new BodyOnCommand(Entity.Id));

            Entity.PostCommand(new PlayAnimCommand(Entity.Id, animationId));
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, "Door - Closing"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
        }

        #endregion Public Methods

    }
}
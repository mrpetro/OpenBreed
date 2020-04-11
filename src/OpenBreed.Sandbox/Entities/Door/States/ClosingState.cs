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
    public class ClosingState : IStateEx<FunctioningState, FunctioningImpulse>
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

        public int Id => (int)(ValueType)FunctioningState.Closing;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            entity.PostCommand(new SpriteOnCommand(entity.Id));
            entity.PostCommand(new BodyOnCommand(entity.Id));

            entity.PostCommand(new PlayAnimCommand(entity.Id, animationId));
            entity.PostCommand(new TextSetCommand(entity.Id, 0, "Door - Closing"));
        }

        public void LeaveState(IEntity entity)
        {
        }

        #endregion Public Methods

    }
}
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using System.Linq;
using OpenBreed.Core.Common.Systems;

using OpenBreed.Core.Modules.Physics.Commands;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Sandbox.Entities.Door.States;
using System;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpenedState : IStateEx<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly int stampId;

        #endregion Private Fields

        #region Public Constructors

        public OpenedState(int stampId)
        {
            this.stampId = stampId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Opened;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            entity.PostCommand(new SpriteOffCommand(entity.Id));
            entity.PostCommand(new BodyOffCommand(entity.Id));

            var pos = entity.GetComponent<PositionComponent>();

            entity.PostCommand(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));
            entity.PostCommand(new TextSetCommand(entity.Id, 0, "Door - Opened"));
        }

        public void LeaveState(IEntity entity)
        {
        }

        #endregion Public Methods

    }
}
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
using OpenBreed.Core.Common.Components;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpenedState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string stampPrefix;

        #endregion Private Fields

        #region Public Constructors

        public OpenedState()
        {
            this.stampPrefix = "Tiles/Stamps";
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

            //entity.PostCommand(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));

            var className = entity.GetComponent<ClassComponent>().Name;
            var worldId = entity.GetComponent<WorldComponent>().WorldId;
            var stateName = entity.Core.StateMachines.GetStateName(FsmId, Id);
            var stampId = entity.Core.Rendering.Stamps.GetByName($"{stampPrefix}/{className}/{stateName}").Id;
            entity.PostCommand(new PutStampCommand(worldId, stampId, 0, pos.Value));


            entity.PostCommand(new TextSetCommand(entity.Id, 0, "Door - Opened"));
        }

        public void LeaveState(IEntity entity)
        {
        }

        #endregion Public Methods

    }
}
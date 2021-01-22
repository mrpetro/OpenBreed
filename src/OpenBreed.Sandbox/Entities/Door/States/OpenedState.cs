using OpenBreed.Systems.Rendering.Commands;
using OpenTK;
using System.Linq;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using OpenBreed.Components.Common;
using OpenBreed.Rendering.Interface;
using OpenBreed.Systems.Physics.Commands;
using OpenBreed.Fsm;
using OpenBreed.Ecsw.Entities;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpenedState : IState
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

        public void EnterState(Entity entity)
        {
            entity.Core.Commands.Post(new SpriteOffCommand(entity.Id));
            entity.Core.Commands.Post(new BodyOffCommand(entity.Id));

            var pos = entity.Get<PositionComponent>();

            //entity.PostCommand(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));

            var className = entity.Get<ClassComponent>().Name;
            var stateName = entity.Core.GetManager<IFsmMan>().GetStateName(FsmId, Id);
            var stampId = entity.Core.GetModule<IRenderModule>().Stamps.GetByName($"{stampPrefix}/{className}/{stateName}").Id;
            entity.Core.Commands.Post(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));


            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, "Door - Opened"));
        }

        public void LeaveState(Entity entity)
        {
        }

        #endregion Public Methods

    }
}
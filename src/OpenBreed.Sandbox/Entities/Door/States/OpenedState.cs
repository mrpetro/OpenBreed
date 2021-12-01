using OpenTK;
using System.Linq;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Rendering.Interface;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpenedState : IState
    {
        #region Private Fields

        private readonly IFsmMan fsmMan;
        private readonly IStampMan stampMan;

        #endregion Private Fields

        #region Public Constructors

        public OpenedState(IFsmMan fsmMan, IStampMan stampMan)
        {
            this.fsmMan = fsmMan;
            this.stampMan = stampMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Opened;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            var pos = entity.Get<PositionComponent>();
            var metadata = entity.Get<MetadataComponent>();
            var level = metadata.Level;
            var className = metadata.Name;
            var flavor = metadata.Flavor;
            var stateName = fsmMan.GetStateName(FsmId, Id);
            var stampId = stampMan.GetByName($"{level}/{className}/{flavor}/{stateName}").Id;

            entity.SetSpriteOff();
            entity.SetBodyOff();
            entity.PutStamp(stampId, 0, pos.Value);
            //entity.SetText(0, "Door - Opened");
        }

        public void LeaveState(Entity entity)
        {
        }

        #endregion Public Methods

    }
}
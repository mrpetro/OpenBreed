using OpenTK;
using System.Linq;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Animation.Interface;

namespace OpenBreed.Sandbox.Components.States
{
    public class ClosingState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly IFsmMan fsmMan;
        private readonly IStampMan stampMan;
        private readonly IClipMan<Entity> clipMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public ClosingState(IFsmMan fsmMan, IStampMan stampMan, IClipMan<Entity> clipMan, ITriggerMan triggerMan)
        {
            this.fsmMan = fsmMan;
            this.stampMan = stampMan;
            this.clipMan = clipMan;
            this.triggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Closing;
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
            var clipId = clipMan.GetByName($"{level}/{className}/{stateName}/{flavor}").Id;
            var stampId = stampMan.GetByName($"{level}/{className}/{flavor}/Closed").Id;

            entity.SetSpriteOn();
            entity.SetBodyOn();
            entity.PlayAnimation(0, clipId);
            //entity.SetText(0, "Door - Closing");
            entity.PutStamp(stampId, 0, pos.Value);

            triggerMan.OnEntityAnimFinished(entity, OnAnimStopped, singleTime: true);
        }

        public void LeaveState(Entity entity)
        {
            entity.SetSpriteOff();
        }

        private void OnAnimStopped(Entity entity, AnimFinishedEventArgs e)
        {
            entity.SetState(FsmId, (int)FunctioningImpulse.StopClosing);
        }

        #endregion Public Methods

    }
}
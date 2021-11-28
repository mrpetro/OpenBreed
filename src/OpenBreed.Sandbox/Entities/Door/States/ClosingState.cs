using OpenTK;
using System.Linq;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Commands;
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
        private readonly IClipMan clipMan;

        #endregion Private Fields

        #region Public Constructors

        public ClosingState(IFsmMan fsmMan, IStampMan stampMan, IClipMan clipMan)
        {
            this.fsmMan = fsmMan;
            this.stampMan = stampMan;
            this.clipMan = clipMan;
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
            var metadata = entity.Get<ClassComponent>();
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

            entity.Subscribe<AnimFinishedEventArgs>(OnAnimStopped);
        }

        public void LeaveState(Entity entity)
        {
            entity.SetSpriteOff();

            entity.Unsubscribe<AnimFinishedEventArgs>(OnAnimStopped);
        }

        private void OnAnimStopped(object sender, AnimFinishedEventArgs e)
        {
            var entity = sender as Entity;
            entity.SetState(FsmId, (int)FunctioningImpulse.StopClosing);
        }

        #endregion Public Methods

    }
}
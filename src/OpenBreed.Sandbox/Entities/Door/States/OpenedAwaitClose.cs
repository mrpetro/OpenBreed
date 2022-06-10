using OpenTK;
using System.Linq;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Rendering.Interface;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpenedAwaitClose : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly IFsmMan fsmMan;
        private readonly IStampMan stampMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public OpenedAwaitClose(IFsmMan fsmMan, IStampMan stampMan, ITriggerMan triggerMan)
        {
            this.fsmMan = fsmMan;
            this.stampMan = stampMan;
            this.triggerMan = triggerMan;
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
            entity.PutStampAtPosition(stampId, 0, pos.Value);
            //entity.SetText(0, "Door - Opened");

            triggerMan.OnEntityTimerElapsed(entity, OnTimerElapsed, singleTime: true);
            triggerMan.OnEntityTimerUpdate(entity, OnTimerUpdate, singleTime: true);

            entity.StartTimer(0, 5.0);
        }

        private void OnTimerElapsed(Entity entity, TimerElapsedEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            entity.SetState(FsmId, (int)FunctioningImpulse.Close);
        }

        private void OnTimerUpdate(Entity entity, TimerUpdateEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            var tcp = entity.Get<TimerComponent>();

            var timer = tcp.Items.FirstOrDefault(item => item.TimerId == 0);

            //entity.SetText(0, $"Door - {timer.Interval:F2}s");
        }

        public void LeaveState(Entity entity)
        {
            entity.StopTimer(0);
        }

        #endregion Public Methods

    }
}
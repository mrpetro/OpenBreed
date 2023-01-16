using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using System;
using System.Linq;
using System.Timers;

namespace OpenBreed.Sandbox.Entities.Actor.States.Attacking
{
    public class CooldownState : IState<AttackingState, AttackingImpulse>
    {
        #region Private Fields

        private readonly IFsmMan fsmMan;
        private readonly ITriggerMan triggerMan;
        private Timer timer;

        #endregion Private Fields

        #region Public Constructors

        public CooldownState(IFsmMan fsmMan, ITriggerMan triggerMan)
        {
            this.fsmMan = fsmMan;
            this.triggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)AttackingState.Cooldown;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            var currentStateNames = fsmMan.GetStateNames(entity);

            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            triggerMan.OnEntityTimerElapsed(entity, OnTimerElapsed);

            entity.StartTimer(0, 0.5);
        }

        public void Initialize(IEntity entity)
        {
            timer = new Timer(100);
            timer.AutoReset = false;
        }

        public void LeaveState(IEntity entity)
        {
            entity.StopTimer(0);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnTimerElapsed(IEntity entity, TimerElapsedEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            var cc = entity.Get<ActionControlComponent>();

            //if (cc.Primary)
            //    entity.SetState(FsmId, (int)AttackingImpulse.Shoot);
            //else
            //    entity.SetState(FsmId, (int)AttackingImpulse.Stop);
        }

        #endregion Private Methods
    }
}
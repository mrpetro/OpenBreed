using OpenBreed.Core.Commands;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Commands;
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
        private readonly ICommandsMan commandsMan;
        private Timer timer;

        #endregion Private Fields

        #region Public Constructors

        public CooldownState(IFsmMan fsmMan, ICommandsMan commandsMan)
        {
            this.fsmMan = fsmMan;
            this.commandsMan = commandsMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)AttackingState.Cooldown;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            var currentStateNames = fsmMan.GetStateNames(entity);

            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            entity.Subscribe<TimerElapsedEventArgs>(OnTimerElapsed);

            entity.StartTimer(0, 0.5);
        }

        public void Initialize(Entity entity)
        {
            timer = new Timer(100);
            timer.AutoReset = false;
        }

        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<TimerElapsedEventArgs>(OnTimerElapsed);

            entity.StopTimer(0);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnTimerElapsed(object sender, TimerElapsedEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            var entity = sender as Entity;

            var cc = entity.Get<AttackControlComponent>();

            if (cc.AttackPrimary)
                entity.SetState(FsmId, (int)AttackingImpulse.Shoot);
            else
                entity.SetState(FsmId, (int)AttackingImpulse.Stop);
        }

        #endregion Private Methods
    }
}

using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Systems.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Components.Control;
using OpenTK;
using System;
using System.Linq;
using System.Timers;

namespace OpenBreed.Sandbox.Entities.Actor.States.Attacking
{
    public class CooldownState : IState<AttackingState, AttackingImpulse>
    {
        private Timer timer;

        #region Public Constructors

        public CooldownState()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)AttackingState.Cooldown;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            entity.Core.Commands.Post(new TimerStartCommand(entity.Id, 0, 0.2));
        }

        private void OnTimerElapsed(object sender, TimerElapsedEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            var entity = sender as Entity;

            var cc = entity.Get<AttackControl>();

            if (cc.AttackPrimary)
                entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)AttackingImpulse.Shoot));
            else
                entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)AttackingImpulse.Stop));
        }

        public void Initialize(Entity entity) 
        {
            timer = new Timer(100);
            timer.AutoReset = false;
        }

        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            entity.Core.Commands.Post(new TimerStopCommand(entity.Id, 0));
        }

        #endregion Public Methods

        #region Private Methods



        #endregion Private Methods
    }
}
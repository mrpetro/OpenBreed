
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Control.Components;
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

        public void EnterState(IEntity entity)
        {
            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.PostCommand(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            entity.PostCommand(new TimerStartCommand(entity.Id, 0, 0.2));
        }

        private void OnTimerElapsed(object sender, TimerElapsedEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            var entity = sender as IEntity;

            var cc = entity.GetComponent<AttackControl>();

            if(cc.AttackPrimary)
                entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)AttackingImpulse.Shoot));
            else
                entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)AttackingImpulse.Stop));
        }

        public void Initialize(IEntity entity) 
        {
            timer = new Timer(100);
            timer.AutoReset = false;
        }

        public void LeaveState(IEntity entity)
        {
            entity.Unsubscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            entity.PostCommand(new TimerStopCommand(entity.Id, 0));
        }

        #endregion Public Methods

        #region Private Methods



        #endregion Private Methods
    }
}
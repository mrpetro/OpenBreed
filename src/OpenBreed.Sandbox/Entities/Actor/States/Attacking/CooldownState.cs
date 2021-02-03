﻿
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Components.Control;
using OpenTK;
using System;
using System.Linq;
using System.Timers;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Wecs.Systems.Core.Events;

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
            var currentStateNames = entity.Core.GetManager<IFsmMan>().GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            entity.Core.Commands.Post(new TimerStartCommand(entity.Id, 0, 0.2));
        }

        private void OnTimerElapsed(object sender, TimerElapsedEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            var entity = sender as Entity;

            var cc = entity.Get<AttackControlComponent>();

            if (cc.AttackPrimary)
                entity.Core.Commands.Post(new SetEntityStateCommand(entity.Id, FsmId, (int)AttackingImpulse.Shoot));
            else
                entity.Core.Commands.Post(new SetEntityStateCommand(entity.Id, FsmId, (int)AttackingImpulse.Stop));
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

using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
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

        public IEntity Entity { get; private set; }
        public AttackingState Id => AttackingState.Cooldown;

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            //Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            Entity.Subscribe<ControlFireChangedEvenrArgs>(OnControlFireChanged);

            timer.Start();
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "AttackingState", "Shoot"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            timer = new Timer(100);
            timer.AutoReset = false;
        }

        public void LeaveState()
        {
            timer.Stop();
            timer.Elapsed -= Timer_Elapsed;

            Entity.Unsubscribe<ControlFireChangedEvenrArgs>(OnControlFireChanged);
        }

        private void OnControlFireChanged(object sender, ControlFireChangedEvenrArgs e)
        {
            if (e.Fire)
                Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "AttackingState", "Shoot"));
            else
                Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "AttackingState", "Stop"));
        }

        public AttackingState Process(AttackingImpulse impulse, object[] arguments)
        {
            switch (impulse)
            {
                case AttackingImpulse.Stop:
                    {
                        return AttackingState.Idle;
                    }
                case AttackingImpulse.Shoot:
                    {
                        return AttackingState.Shooting;
                    }
                default:
                    break;
            }

            return AttackingState.Cooldown;
        }

        #endregion Public Methods

        #region Private Methods



        #endregion Private Methods
    }
}
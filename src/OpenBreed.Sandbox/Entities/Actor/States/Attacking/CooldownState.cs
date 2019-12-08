using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenTK;
using System;
using System.Linq;
using System.Timers;

namespace OpenBreed.Sandbox.Entities.Actor.States.Attacking
{
    public class CooldownState : IState
    {
        private Timer timer;

        #region Public Constructors

        public CooldownState(string id)
        {
            Name = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            //Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostMsg(new TextSetMsg(Entity.Id, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            Entity.Subscribe(ControlFireChangedEvent.TYPE, OnControlFireChanged);

            timer.Start();
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Entity.PostMsg(new StateChangeMsg(Entity.Id, "Attacking", "Shoot"));
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

            Entity.Unsubscribe(ControlFireChangedEvent.TYPE, OnControlFireChanged);
        }

        private void OnControlFireChanged(object sender, IEvent e)
        {
            HandleControlFireChangedEvent((ControlFireChangedEvent)e);
        }

        private void HandleControlFireChangedEvent(ControlFireChangedEvent systemEvent)
        {
            if (systemEvent.Fire)
                Entity.PostMsg(new StateChangeMsg(Entity.Id, "Attacking", "Shoot"));
            else
                Entity.PostMsg(new StateChangeMsg(Entity.Id, "Attacking", "Stop"));
        }


        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Stop":
                    {
                        return "Idle";
                    }
                case "Shoot":
                    {
                        return "Shooting";
                    }
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods



        #endregion Private Methods
    }
}
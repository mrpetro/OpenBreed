using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities.Actor.States.Attacking
{
    public class IdleState : IState
    {
        public IEntity Entity { get; private set; }

        public IdleState(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public void EnterState(object[] arguments)
        {
            // Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostMsg(new TextSetMsg(Entity, "Hero - Idle"));

            Entity.Subscribe(ControlFireChangedEvent.TYPE, OnControlFireChanged);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(ControlFireChangedEvent.TYPE, OnControlFireChanged);
        }

        private void OnControlFireChanged(object sender, IEvent e)
        {
            HandleControlFireChangedEvent((ControlFireChangedEvent)e);
        }

        private void HandleControlFireChangedEvent(ControlFireChangedEvent systemEvent)
        {
            if (systemEvent.Fire)
                Entity.PostMsg(new StateChangeMsg(Entity, "Attacking", "Shoot"));
            else
                Entity.PostMsg(new StateChangeMsg(Entity, "Attacking", "Idle"));
        }


        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Shoot":
                    {
                        return "Shooting";
                    }
                default:
                    break;
            }

            return null;
        }
    }
}

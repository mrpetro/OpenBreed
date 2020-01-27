
using NLua;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Control.Events;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Actor.States.Attacking
{
    public class IdleState : IState
    {
        public IEntity Entity { get; private set; }

        public IdleState(string id)
        {
            Name = id;
        }

        public string Name { get; }

        public void EnterState()
        {
            // Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostCommand(new TextSetCommand(Entity.Id, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            Entity.Subscribe(ControlEventTypes.CONTROL_FIRE_CHANGED, OnControlFireChanged);


            var s = (LuaTable)Entity.Core.Scripts.GetObject("AtackingFsm.IdleState");
            var r = ((LuaFunction)s["Enter"]).Call(Entity);

        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(ControlEventTypes.CONTROL_FIRE_CHANGED, OnControlFireChanged);
        }

        private void OnControlFireChanged(object sender, EventArgs eventArgs)
        {
            HandleControlFireChangedEvent((ControlFireChangedEvent)eventArgs);
        }

        private void HandleControlFireChangedEvent(ControlFireChangedEvent systemEvent)
        {
            if (systemEvent.Fire)
                Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "Attacking", "Shoot"));
            else
                Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "Attacking", "Stop"));
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

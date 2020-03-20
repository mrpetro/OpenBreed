
using NLua;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
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
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            Entity.Subscribe<ControlFireChangedEvenrArgs>(OnControlFireChanged);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe<ControlFireChangedEvenrArgs>(OnControlFireChanged);
        }

        private void OnControlFireChanged(object sender, ControlFireChangedEvenrArgs eventArgs)
        {
            if (eventArgs.Fire)
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

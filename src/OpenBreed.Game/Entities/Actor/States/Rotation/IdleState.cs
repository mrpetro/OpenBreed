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

namespace OpenBreed.Game.Entities.Actor.States.Rotation
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
            Entity.PostMsg(new TextSetMsg(Entity, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            Entity.Subscribe(ControlDirectionChangedEvent.TYPE, OnControlDirectionChanged);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(ControlDirectionChangedEvent.TYPE, OnControlDirectionChanged);
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Rotate":
                    {
                        return "Rotating";
                    }
                default:
                    break;
            }

            return null;
        }

        private void OnControlDirectionChanged(object sender, IEvent e)
        {
            HandleControlDirectionChangedEvent((ControlDirectionChangedEvent)e);
        }

        private void HandleControlDirectionChangedEvent(ControlDirectionChangedEvent systemEvent)
        {
            if (systemEvent.Direction != Vector2.Zero)
            {
                var dir = Entity.Components.OfType<Direction>().First();

                if (dir.Value != systemEvent.Direction)
                {
                    dir.Value = systemEvent.Direction;
                    Entity.PostMsg(new StateChangeMsg(Entity, "Rotation", "Rotate"));
                }
            }

        }
    }
}

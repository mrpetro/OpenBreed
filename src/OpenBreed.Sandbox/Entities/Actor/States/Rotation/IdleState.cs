
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

namespace OpenBreed.Sandbox.Entities.Actor.States.Rotation
{
    public class IdleState : IState<RotationState>
    {
        public IEntity Entity { get; private set; }

        public IdleState()
        {
        }

        public RotationState Id => RotationState.Idle;

        public void EnterState()
        {
            // Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            Entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        public RotationState Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Rotate":
                    {
                        return RotationState.Rotating;
                    }
                default:
                    break;
            }

            return Id;
        }

        private void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            if (e.Direction != Vector2.Zero)
            {
                var dir = Entity.GetComponent<DirectionComponent>();

                if (dir.Value != e.Direction)
                {
                    dir.Value = e.Direction;
                    Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "RotationState", "Rotate"));
                }
            }
        }
    }
}


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
    public class IdleState : IStateEx<RotationState, RotationImpulse>
    {
        public IdleState()
        {
        }

        public int Id => (int)RotationState.Idle;
        public int FsmId { get; set; }

        public void EnterState(IEntity entity)
        {
            // Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.PostCommand(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        public void LeaveState(IEntity entity)
        {
            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        private void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            var entity = sender as IEntity;

            if (e.Direction != Vector2.Zero)
            {
                var dir = entity.GetComponent<DirectionComponent>();

                if (dir.Value != e.Direction)
                {
                    dir.Value = e.Direction;
                    entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)RotationImpulse.Rotate));
                }
            }
        }
    }
}

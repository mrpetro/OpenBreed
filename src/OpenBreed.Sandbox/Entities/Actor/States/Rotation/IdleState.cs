using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Helpers;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Sandbox.Entities.Actor.States.Rotation
{
    public class IdleState : IState<RotationState, RotationImpulse>
    {
        public IdleState()
        {
        }

        public int Id => (int)RotationState.Idle;
        public int FsmId { get; set; }

        public void EnterState(Entity entity)
        {
            // Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            var currentStateNames = entity.Core.GetManager<IFsmMan>().GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            //entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        public void LeaveState(Entity entity)
        {
            //entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        private void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            var entity = sender as Entity;

            if (e.Direction != Vector2.Zero)
            {
                var angularPos = entity.Get<AngularPositionComponent>();

                if (angularPos.GetDirection() != e.Direction)
                {
                    //var aPos3 = new Vector3(angularPos.GetDirection());
                    //var dPos3 = new Vector3(e.Direction);
                    //var newVec = Vector3Extension.RotateTowards(aPos3, dPos3, 0.4f, 1.0f);
                    var angularThrust = entity.Get<AngularVelocityComponent>();
                    angularThrust.SetDirection(new Vector2(e.Direction.X, e.Direction.Y));
                    //dir.SetDirection(e.Direction);
                    entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)RotationImpulse.Rotate));
                }
            }
        }
    }
}

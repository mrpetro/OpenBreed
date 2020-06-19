
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
    public class IdleState : IState<AttackingState, AttackingImpulse>
    {
        public IdleState()
        {
        }

        public int Id => (int)AttackingState.Idle;
        public int FsmId { get; set; }

        public void EnterState(Entity entity)
        {
            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlFireChangedEvenrArgs>(OnControlFireChanged);
        }

        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<ControlFireChangedEvenrArgs>(OnControlFireChanged);
        }

        private void OnControlFireChanged(object sender, ControlFireChangedEvenrArgs eventArgs)
        {
            var entity = sender as Entity;

            if (eventArgs.Fire)
                entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)AttackingImpulse.Shoot));
        }

    }
}

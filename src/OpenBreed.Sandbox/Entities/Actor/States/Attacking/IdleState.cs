using NLua;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Control.Extensions;

namespace OpenBreed.Sandbox.Entities.Actor.States.Attacking
{
    public class IdleState : IState<AttackingState, AttackingImpulse>
    {
        private readonly IFsmMan fsmMan;
        private readonly ITriggerMan triggerMan;

        public IdleState(IFsmMan fsmMan, ITriggerMan triggerMan)
        {
            this.fsmMan = fsmMan;
            this.triggerMan = triggerMan;
        }

        public int Id => (int)AttackingState.Idle;
        public int FsmId { get; set; }

        public void EnterState(Entity entity)
        {
            var currentStateNames = fsmMan.GetStateNames(entity);

            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            triggerMan.OnEntityControlFireChanged(entity, OnControlFireChanged, singleTime: true);
        }

        public void LeaveState(Entity entity)
        {
        }

        private void OnControlFireChanged(Entity entity, ControlFireChangedEventArgs eventArgs)
        {
            if (eventArgs.Fire)
                entity.SetState(FsmId, (int)AttackingImpulse.Shoot);
        }

    }
}

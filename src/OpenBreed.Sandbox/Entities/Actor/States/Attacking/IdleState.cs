
using NLua;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Systems.Rendering.Commands;
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
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Fsm.Extensions;

namespace OpenBreed.Sandbox.Entities.Actor.States.Attacking
{
    public class IdleState : IState<AttackingState, AttackingImpulse>
    {
        private readonly IFsmMan fsmMan;
        private readonly ICommandsMan commandsMan;

        public IdleState(IFsmMan fsmMan, ICommandsMan commandsMan)
        {
            this.fsmMan = fsmMan;
            this.commandsMan = commandsMan;
        }

        public int Id => (int)AttackingState.Idle;
        public int FsmId { get; set; }

        public void EnterState(Entity entity)
        {
            var currentStateNames = fsmMan.GetStateNames(entity);
            commandsMan.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));
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
                entity.SetState(FsmId, (int)AttackingImpulse.Shoot);
        }

    }
}

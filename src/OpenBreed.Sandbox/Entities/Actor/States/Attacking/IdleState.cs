
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
    public class IdleState : IStateEx<AttackingState, AttackingImpulse>
    {
        public IdleState()
        {
        }

        public int Id => (int)AttackingState.Idle;
        public int FsmId { get; set; }

        public void EnterState(IEntity entity)
        {
            Console.WriteLine("Enter Idle");
            entity.PostCommand(new TextSetCommand(entity.Id, 0, String.Join(", ", entity.CurrentStateNames.ToArray())));

            entity.Subscribe<ControlFireChangedEvenrArgs>(OnControlFireChanged);
        }

        public void LeaveState(IEntity entity)
        {
            Console.WriteLine("Leave Idle");
            entity.Unsubscribe<ControlFireChangedEvenrArgs>(OnControlFireChanged);
        }

        private void OnControlFireChanged(object sender, ControlFireChangedEvenrArgs eventArgs)
        {
            var entity = sender as IEntity;

            if (eventArgs.Fire)
                entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)AttackingImpulse.Shoot));
        }

    }
}

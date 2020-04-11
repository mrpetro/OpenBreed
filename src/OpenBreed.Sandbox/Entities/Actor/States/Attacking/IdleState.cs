
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
        public IEntity Entity { get; private set; }

        public IdleState()
        {
        }

        public AttackingState Id => AttackingState.Idle;

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
                //Entity.Impulse<AttackingState, AttackingImpulse>(AttackingImpulse.Shoot);
                Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "AttackingState", "Shoot"));
            else
                //Entity.Impulse<AttackingState, AttackingImpulse>(AttackingImpulse.Stop);
                Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "AttackingState", "Stop"));
        }

    }
}

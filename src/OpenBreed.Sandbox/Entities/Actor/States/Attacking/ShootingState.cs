
using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor.States.Attacking
{
    public class ShootingState : IState<AttackingState, AttackingImpulse>
    {
        #region Public Constructors

        public ShootingState()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)AttackingState.Shooting;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            //Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, string.Join(", ", currentStateNames.ToArray())));

            var pos = entity.Get<PositionComponent>().Value;
            pos += new Vector2(8, 8);
            var direction = entity.Get<AngularPositionComponent>().GetDirection();
            direction.Normalize();
            direction *= 500.0f;
            ProjectileHelper.AddProjectile(entity.Core, entity.World, pos.X, pos.Y, direction.X, direction.Y);

            //Entity.Impulse<AttackingState, AttackingImpulse>(AttackingImpulse.Wait);
            //entity.PostCommand(new EntitySetStateCommand(entity.Id, "AttackingState", "Wait"));
            entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)AttackingImpulse.Wait));
        }

        public void LeaveState(Entity entity)
        {
            //Entity.Unsubscribe(ControlFireChangedEvent.TYPE, OnControlFireChanged);
        }

        //private void OnControlFireChanged(object sender, IEvent e)
        //{
        //    HandleControlFireChangedEvent((ControlFireChangedEvent)e);
        //}

        //private void HandleControlFireChangedEvent(ControlFireChangedEvent systemEvent)
        //{
        //    if (systemEvent.Fire)
        //        Entity.PostMsg(new StateChangeMsg(Entity, "Attacking", "Stop"));
        //    else
        //        Entity.PostMsg(new StateChangeMsg(Entity, "Attacking", "Cooldown"));
        //}

        #endregion Public Methods

        #region Private Methods



        #endregion Private Methods
    }
}
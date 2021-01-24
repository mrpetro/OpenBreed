using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Sandbox.Entities.Actor.States.Attacking;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Wecs.Systems.Core.Events;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorAttackingHelper
    {
        #region Public Methods

        public static void CreateFsm(ICore core)
        {
            var stateMachine = core.GetManager<IFsmMan>().Create<AttackingState, AttackingImpulse>("Actor.Attacking");

            stateMachine.AddState(new States.Attacking.ShootingState());
            stateMachine.AddState(new States.Attacking.IdleState());
            stateMachine.AddState(new States.Attacking.CooldownState());

            stateMachine.AddTransition(AttackingState.Shooting, AttackingImpulse.Stop, AttackingState.Idle);
            stateMachine.AddTransition(AttackingState.Shooting, AttackingImpulse.Wait, AttackingState.Cooldown);
            stateMachine.AddTransition(AttackingState.Cooldown, AttackingImpulse.Stop, AttackingState.Idle);
            stateMachine.AddTransition(AttackingState.Cooldown, AttackingImpulse.Shoot, AttackingState.Shooting);
            stateMachine.AddTransition(AttackingState.Idle, AttackingImpulse.Shoot, AttackingState.Shooting);

            //stateMachine.AddOnEnterState(AttackingState.Shooting, AttackingImpulse.Shoot, OnAttackingEnterShootingWithShoot);
            //stateMachine.AddOnEnterState(AttackingState.Cooldown, AttackingImpulse.Wait, OnAttackingEnterCooldownWithWait);
            //stateMachine.AddOnEnterState(AttackingState.Idle, AttackingImpulse.Stop, OnAttackingEnterIdleWithStop);
            //stateMachine.AddOnLeaveState(AttackingState.Cooldown, AttackingImpulse.Stop, OnAttackingLeaveCooldownWithStop);
            //stateMachine.AddOnLeaveState(AttackingState.Cooldown, AttackingImpulse.Shoot, OnAttackingLeaveCooldownWithShoot);
            //stateMachine.AddOnLeaveState(AttackingState.Idle, AttackingImpulse.Shoot, OnAttackingLeaveIdleWithShoot);
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnAttackingLeaveCooldownWithStop(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);
            entity.Unsubscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            entity.Core.Commands.Post(new TimerStopCommand(entity.Id, 0));
        }

        private static void OnAttackingLeaveCooldownWithShoot(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);
            entity.Unsubscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            entity.Core.Commands.Post(new TimerStopCommand(entity.Id, 0));
        }

        private static void OnAttackingLeaveIdleWithShoot(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);
            entity.Unsubscribe<ControlFireChangedEvenrArgs>(OnControlFireChanged);
        }

        private static void OnAttackingEnterIdleWithStop(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);

            var currentStateNames = entity.Core.GetManager<IFsmMan>().GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlFireChangedEvenrArgs>(OnControlFireChanged);
        }

        private static void OnAttackingEnterCooldownWithWait(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);

            var currentStateNames = entity.Core.GetManager<IFsmMan>().GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            entity.Core.Commands.Post(new TimerStartCommand(entity.Id, 0, 0.2));
        }

        private static void OnTimerElapsed(object sender, TimerElapsedEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            var entity = sender as Entity;

            var cc = entity.Get<AttackControl>();

            var fsmId = entity.Core.GetManager<IFsmMan>().GetByName("Actor.Attacking").Id;

            if (cc.AttackPrimary)
                entity.Core.Commands.Post(new SetEntityStateCommand(entity.Id, fsmId, (int)AttackingImpulse.Shoot));
            else
                entity.Core.Commands.Post(new SetEntityStateCommand(entity.Id, fsmId, (int)AttackingImpulse.Stop));
        }

        private static void OnControlFireChanged(object sender, ControlFireChangedEvenrArgs eventArgs)
        {
            var entity = sender as Entity;
            var fsmId = entity.Core.GetManager<IFsmMan>().GetByName("Actor.Attacking").Id;

            if (eventArgs.Fire)
                entity.Core.Commands.Post(new SetEntityStateCommand(entity.Id, fsmId, (int)AttackingImpulse.Shoot));
        }

        private static void OnAttackingEnterShootingWithShoot(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);

            //Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            var currentStateNames = entity.Core.GetManager<IFsmMan>().GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, string.Join(", ", currentStateNames.ToArray())));

            var pos = entity.Get<PositionComponent>().Value;
            pos += new Vector2(8, 8);
            var direction = entity.Get<AngularPositionComponent>().GetDirection();
            direction.Normalize();
            direction *= 500.0f;
            ProjectileHelper.AddProjectile(entity.Core, entity.World, pos.X, pos.Y, direction.X, direction.Y);

            entity.Core.Commands.Post(new SetEntityStateCommand(entity.Id, fsmId, (int)AttackingImpulse.Wait));
        }

        #endregion Private Methods
    }
}
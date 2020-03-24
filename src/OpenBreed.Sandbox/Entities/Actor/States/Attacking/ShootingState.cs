
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Systems.Components;
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

        public IEntity Entity { get; private set; }
        public AttackingState Id => AttackingState.Shooting;

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            //Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            var pos = Entity.GetComponent<PositionComponent>().Value;
            pos += new Vector2(8,8);
            var direction = Entity.GetComponent<DirectionComponent>().Value;
            direction.Normalize();
            direction *= 500.0f;
            ProjectileHelper.AddProjectile(Entity.Core, Entity.World, pos.X, pos.Y, direction.X, direction.Y);

            Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "AttackingState", "Wait"));

        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
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

        public AttackingState Process(AttackingImpulse impulse, object[] arguments)
        {
            switch (impulse)
            {
                case AttackingImpulse.Wait:
                    {
                        return AttackingState.Cooldown;
                    }
                case AttackingImpulse.Stop:
                    {
                        return AttackingState.Idle;
                    }
                default:
                    break;
            }

            return Id;
        }

        #endregion Public Methods

        #region Private Methods



        #endregion Private Methods
    }
}
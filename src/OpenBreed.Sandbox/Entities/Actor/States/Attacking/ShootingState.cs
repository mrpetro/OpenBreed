﻿
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
    public class ShootingState : IState
    {
        #region Public Constructors

        public ShootingState(string name)
        {
            Name = name;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            //Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostCommand(new TextSetCommand(Entity.Id, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            var pos = Entity.GetComponent<Position>().Value;
            pos += new Vector2(8,8);
            var direction = Entity.GetComponent<Direction>().Value;
            direction.Normalize();
            direction *= 500.0f;
            ProjectileHelper.AddProjectile(Entity.Core, Entity.World, pos.X, pos.Y, direction.X, direction.Y);

            Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "Attacking", "Wait"));

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

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Wait":
                    {
                        return "Cooldown";
                    }
                case "Stop":
                    {
                        return "Idle";
                    }
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods



        #endregion Private Methods
    }
}
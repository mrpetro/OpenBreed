﻿using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Game.Entities.Projectile;
using OpenTK;
using System.Linq;

namespace OpenBreed.Game.Entities.Actor.States.Attacking
{
    public class ShootingState : IState
    {
        #region Public Constructors

        public ShootingState(string id)
        {
            Id = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Id { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(object[] arguments)
        {
            //Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostMsg(new TextSetMsg(Entity, "Hero - Shooting"));


            var pos = Entity.Components.OfType<Position>().FirstOrDefault().Value;
            pos += new Vector2(8,8);
            var velocity = Entity.Components.OfType<Direction>().FirstOrDefault().Value;
            velocity.Normalize();
            velocity *= 500.0f;
            ProjectileHelper.AddProjectile(Entity.Core, Entity.World, pos.X, pos.Y, velocity.X, velocity.Y);

            Entity.PostMsg(new StateChangeMsg(Entity, "Attacking", "Wait"));

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
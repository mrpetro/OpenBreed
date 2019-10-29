using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Game.Entities.Projectile;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Game.Entities.Pickable.States
{
    public class PickingState : IState
    {
        #region Private Fields

        private int stampId;

        #endregion Private Fields

        #region Public Constructors

        public PickingState(string id, int stampId)
        {
            Name = id;
            this.stampId = stampId;
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
            Entity.PostMsg(new TextSetMsg(Entity, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            var pos = Entity.Components.OfType<Position>().FirstOrDefault().Value;
            pos += new Vector2(8, 8);
            var direction = Entity.Components.OfType<Direction>().FirstOrDefault().Value;
            direction.Normalize();
            direction *= 500.0f;
            ProjectileHelper.AddProjectile(Entity.Core, Entity.World, pos.X, pos.Y, direction.X, direction.Y);

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
    }
}
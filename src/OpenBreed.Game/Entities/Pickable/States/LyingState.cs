using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using System;
using System.Linq;

namespace OpenBreed.Game.Entities.Pickable.States
{
    public class LyingState : IState
    {
        #region Private Fields

        private int stampId;

        #endregion Private Fields

        #region Public Constructors

        public LyingState(string id, int stampId)
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
            // Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostMsg(new TextSetMsg(Entity, String.Join(", ", Entity.CurrentStateNames.ToArray())));
            var pos = Entity.Components.OfType<IPosition>().FirstOrDefault();
            Entity.PostMsg(new PutStampMsg(Entity, stampId, 0, pos.Value));
            Entity.Subscribe(CollisionEvent.TYPE, OnCollision);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(CollisionEvent.TYPE, OnCollision);
        }

        private void OnCollision(object sender, IEvent e)
        {
            HandleCollisionEvent((CollisionEvent)e);
        }

        private void HandleCollisionEvent(CollisionEvent e)
        {
            Entity.PostMsg(new StateChangeMsg(Entity, "Functioning", "Pick"));
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Pick":
                    {
                        return "Picking";
                    }
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnControlFireChanged(object sender, IEvent e)
        {
            HandleControlFireChangedEvent((ControlFireChangedEvent)e);
        }

        private void HandleControlFireChangedEvent(ControlFireChangedEvent systemEvent)
        {
            if (systemEvent.Fire)
                Entity.PostMsg(new StateChangeMsg(Entity, "Attacking", "Shoot"));
        }

        #endregion Private Methods
    }
}
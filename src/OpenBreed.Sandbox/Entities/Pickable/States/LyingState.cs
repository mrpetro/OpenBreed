
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Pickable.States
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
            Entity.PostCommand(new TextSetCommand(Entity.Id, String.Join(", ", Entity.CurrentStateNames.ToArray())));
            var pos = Entity.Components.OfType<Position>().FirstOrDefault();
            Entity.PostCommand(new PutStampCommand(Entity.World.Id, stampId, 0, pos.Value));
            Entity.Subscribe(PhysicsEventTypes.COLLISION_OCCURRED, OnCollision);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(PhysicsEventTypes.COLLISION_OCCURRED, OnCollision);
        }

        private void OnCollision(object sender, EventArgs e)
        {
            HandleCollisionEvent((CollisionEventArgs)e);
        }

        private void HandleCollisionEvent(CollisionEventArgs e)
        {
            Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "Functioning", "Pick"));
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

        #endregion Private Methods
    }
}
using OpenBreed.Core.Common.Components;

using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using System;
using System.Linq;
using OpenBreed.Core.Commands;

namespace OpenBreed.Sandbox.Components.States
{
    public class ClosedState : IState
    {
        #region Private Fields

        private readonly int stampId;

        #endregion Private Fields

        #region Public Constructors

        public ClosedState(string id, int stampId)
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
            Entity.PostCommand(new SpriteOffCommand(Entity.Id));

            var pos = Entity.GetComponent<Position>();
            Entity.PostCommand(new PutStampCommand(Entity.World.Id, stampId, 0, pos.Value));
            Entity.PostCommand(new TextSetCommand(Entity.Id, "Door - Closed"));

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

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Open":
                    return "Opening";

                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnCollision(object sender, EventArgs eventArgs)
        {
            HandleCollisionEvent((CollisionEventArgs)eventArgs);
        }

        private void HandleCollisionEvent(CollisionEventArgs e)
        {
            Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "Functioning", "Open"));
        }

        #endregion Private Methods
    }
}
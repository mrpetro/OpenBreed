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

namespace OpenBreed.Sandbox.Entities.Button.States
{
    public class PressedState : IState
    {
        public const string NAME = "Pressed";

        #region Private Fields

        private readonly int stampId;

        #endregion Private Fields

        #region Public Constructors

        public PressedState()
        {
            //Name = id;
            //this.stampId = stampId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Name { get { return NAME; } }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            Entity.PostCommand(new SpriteOffCommand(Entity.Id));

            var pos = Entity.GetComponent<PositionComponent>();
            Entity.PostCommand(new PutStampCommand(Entity.World.Id, stampId, 0, pos.Value));
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, "Door - Closed"));

            Entity.Subscribe<CollisionEventArgs>(OnCollision);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe<CollisionEventArgs>(OnCollision);
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

        private void OnCollision(object sender, CollisionEventArgs eventArgs)
        {
            Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "Functioning", "Open"));
        }

        #endregion Private Methods
    }
}
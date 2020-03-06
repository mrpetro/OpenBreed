using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using System.Linq;
using OpenBreed.Core.Common.Systems;

using OpenBreed.Core.Modules.Physics.Commands;
using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpenedState : IState
    {
        #region Private Fields

        private readonly int stampId;

        #endregion Private Fields

        #region Public Constructors

        public OpenedState(string id, int stampId)
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
            Entity.PostCommand(new BodyOffCommand(Entity.Id));

            var pos = Entity.GetComponent<PositionComponent>();

            Entity.PostCommand(new PutStampCommand(Entity.World.Id, stampId, 0, pos.Value));
            Entity.PostCommand(new TextSetCommand(Entity.Id, "Door - Opened"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Close":
                    return "Closing";
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

    }
}
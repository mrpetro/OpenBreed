using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using System.Linq;

namespace OpenBreed.Game.Components.States
{
    public class ClosedState : IState
    {
        #region Private Fields

        private readonly int leftTileId;
        private readonly int rightTileId;
        private IEntity[] doorParts;

        #endregion Private Fields

        #region Public Constructors

        public ClosedState(string id, int leftTileId, int rightTileId)
        {
            Id = id;
            this.leftTileId = leftTileId;
            this.rightTileId = rightTileId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Id { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            Entity.PostMsg(new SpriteOffMsg(Entity));
            Entity.PostMsg(new TileSetMsg(doorParts[0], leftTileId));
            Entity.PostMsg(new TileSetMsg(doorParts[1], rightTileId));

            Entity.PostMsg(new TextSetMsg(Entity, "Door - Closed"));

            foreach (var doorPart in doorParts)
                doorPart.Subscribe(CollisionEvent.TYPE, OnCollision);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;

            doorParts = Entity.World.Systems.OfType<GroupSystem>().First().GetGroup(Entity).ToArray();
        }

        public void LeaveState()
        {
            foreach (var doorPart in doorParts)
                doorPart.Unsubscribe(CollisionEvent.TYPE, OnCollision);
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

        private void OnCollision(object sender, IEvent e)
        {
            HandleCollisionEvent((CollisionEvent)e);
        }

        private void HandleCollisionEvent(CollisionEvent e)
        {
            Entity.PostMsg(new StateChangeMsg(Entity, "Open"));
        }

        #endregion Private Methods
    }
}
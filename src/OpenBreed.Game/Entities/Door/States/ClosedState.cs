using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
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

        private readonly int leftTileImageId;
        private readonly int rightTileImageId;
        private IEntity[] doorParts;

        #endregion Private Fields

        #region Public Constructors

        public ClosedState(string id, int leftTileId, int rightTileId)
        {
            Id = id;
            this.leftTileImageId = leftTileId;
            this.rightTileImageId = rightTileId;
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
            Entity.PostMsg(new TileSetMsg(doorParts[0], leftTileImageId));
            Entity.PostMsg(new TileSetMsg(doorParts[1], rightTileImageId));

            Entity.PostMsg(new TextSetMsg(Entity, "Door - Closed"));

            foreach (var doorPart in doorParts)
                doorPart.Subscribe(CollisionEvent.TYPE, OnCollision);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;

            var core = entity.Core;
            var world = entity.World;

            //var doorPart1 = core.Entities.Create();
            //doorPart1.Add(GridPosition.Create(x, y));
            //doorPart1.Add(Body.Create(1.0f, 1.0f));
            //doorPart1.Add(AxisAlignedBoxShape.Create(0, 0, 16, 16));
            //doorPart1.Add(GroupPart.Create(door.Id));
            //doorPart1.Add(Tile.Create(tileAtlas.Id));

            //var doorPart2 = core.Entities.Create();
            //doorPart2.Add(GridPosition.Create(x + 1, y));
            //doorPart2.Add(Body.Create(1.0f, 1.0f));
            //doorPart2.Add(AxisAlignedBoxShape.Create(0, 0, 16, 16));
            //doorPart2.Add(Tile.Create(tileAtlas.Id));
            //doorPart2.Add(GroupPart.Create(door.Id));

            //world.AddEntity(doorPart1);
            //world.AddEntity(doorPart2);

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
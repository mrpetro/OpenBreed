using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    [RequireEntityWith(
        typeof(BodyComponent),
        typeof(PositionComponent))]
    [RequireEntityWithout(typeof(VelocityComponent))]
    public class StaticBodiesSystem : SystemBase<StaticBodiesSystem>
    {
        #region Private Fields

        private const int CELL_SIZE = 16;

        private readonly List<int> inactiveStatics = new List<int>();
        private readonly IEntityMan entityMan;
        private readonly IShapeMan shapeMan;
        private readonly IEventsMan eventsMan;
        private readonly IBroadphaseStatic broadphaseGrid;

        #endregion Private Fields

        #region Internal Constructors

        internal StaticBodiesSystem(
            IWorld world,
            IEntityMan entityMan,
            IShapeMan shapeMan,
            IEventsMan eventsMan) :
            base(world)
        {
            this.entityMan = entityMan;
            this.shapeMan = shapeMan;
            this.eventsMan = eventsMan;


            broadphaseGrid = world.GetModule<IBroadphaseStatic>();
        }

        #endregion Internal Constructors

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        public override bool ContainsEntity(IEntity entity) => broadphaseGrid.ContainsItem(entity.Id);

        public override void AddEntity(IEntity entity)
        {
            InsertToGrid(entity);
        }

        public override void RemoveEntity(IEntity entity)
        {
            RemoveFromGrid(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void InsertToGrid(IEntity entity)
        {
            var pos = entity.Get<PositionComponent>();
            var body = entity.Get<BodyComponent>();

            var shape = body.Fixtures.First().Shape;
            var aabb = shape.GetAabb().Translated(pos.Value);

            broadphaseGrid.InsertItem(entity.Id, aabb);
        }

        private void RemoveFromGrid(IEntity entity)
        {
            broadphaseGrid.RemoveStatic(entity.Id);
        }

        #endregion Private Methods
    }
}
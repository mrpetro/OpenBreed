using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class StaticBodiesSystem : SystemBase
    {
        #region Private Fields

        private const int CELL_SIZE = 16;

        private readonly List<int> inactiveStatics = new List<int>();
        private readonly IEntityMan entityMan;
        private readonly IShapeMan shapeMan;
        private readonly IEventsMan eventsMan;
        private IBroadphaseStatic broadphaseGrid;

        #endregion Private Fields

        #region Internal Constructors

        internal StaticBodiesSystem(IEntityMan entityMan, IShapeMan shapeMan, IEventsMan eventsMan)
        {
            this.entityMan = entityMan;
            this.shapeMan = shapeMan;
            this.eventsMan = eventsMan;
            RequireEntityWith<BodyComponent>();
            RequireEntityWith<PositionComponent>();
            RequireEntityWithout<VelocityComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            broadphaseGrid = world.GetModule<IBroadphaseStatic>();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(IEntity entity) => broadphaseGrid.ContainsItem(entity.Id);

        protected override void OnAddEntity(IEntity entity)
        {
            InsertToGrid(entity);
        }

        protected override void OnRemoveEntity(IEntity entity)
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
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class DynamicBodiesAabbUpdaterSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IShapeMan shapeMan;
        private IBroadphaseDynamic broadphaseDynamic;

        #endregion Private Fields

        #region Internal Constructors

        internal DynamicBodiesAabbUpdaterSystem(IShapeMan shapeMan)
        {
            this.shapeMan = shapeMan;

            RequireEntityWith<BodyComponent>();
            RequireEntityWith<PositionComponent>();
            RequireEntityWith<VelocityComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            broadphaseDynamic = world.GetModule<IBroadphaseDynamic>();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, float dt)
        {
            var aabb = Calculate(entity);

            broadphaseDynamic.UpdateItem(entity.Id, aabb);
        }

        protected override void OnAddEntity(Entity entity)
        {
            base.OnAddEntity(entity);

            var aabb = Calculate(entity);
            broadphaseDynamic.InsertItem(entity.Id, aabb);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            base.OnRemoveEntity(entity);

            broadphaseDynamic.RemoveItem(entity.Id);
        }

        #endregion Protected Methods

        #region Private Methods

        private Box2 Calculate(Entity entity)
        {
            var body = entity.Get<BodyComponent>();
            var pos = entity.Get<PositionComponent>();
            var shape = shapeMan.GetById(body.Fixtures.First().ShapeId);
            return shape.GetAabb().Translated(pos.Value);
        }

        #endregion Private Methods
    }
}
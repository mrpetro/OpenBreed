using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    [RequireEntityWith(
        typeof(BodyComponent),
        typeof(PositionComponent),
        typeof(VelocityComponent))]
    public class DynamicBodiesAabbUpdaterSystem : UpdatableSystemBase<DynamicBodiesAabbUpdaterSystem>
    {
        #region Private Fields

        private readonly IShapeMan shapeMan;
        private IBroadphaseDynamic broadphaseDynamic;

        #endregion Private Fields

        #region Internal Constructors

        internal DynamicBodiesAabbUpdaterSystem(
            IWorld world,
            IShapeMan shapeMan) :
            base(world)
        {
            this.shapeMan = shapeMan;

            broadphaseDynamic = world.GetModule<IBroadphaseDynamic>();
        }

        #endregion Internal Constructors

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var aabb = Calculate(entity);

            broadphaseDynamic.UpdateItem(entity.Id, aabb);
        }

        public override void AddEntity(IEntity entity)
        {
            base.AddEntity(entity);

            var aabb = Calculate(entity);
            broadphaseDynamic.InsertItem(entity.Id, aabb);
        }

        public override void RemoveEntity(IEntity entity)
        {
            base.RemoveEntity(entity);

            broadphaseDynamic.RemoveItem(entity.Id);
        }

        #endregion Protected Methods

        #region Private Methods

        private Box2 Calculate(IEntity entity)
        {
            var body = entity.Get<BodyComponent>();
            var pos = entity.Get<PositionComponent>();
            var shape = body.Fixtures.First().Shape;
            return shape.GetAabb().Translated(pos.Value);
        }

        #endregion Private Methods
    }
}
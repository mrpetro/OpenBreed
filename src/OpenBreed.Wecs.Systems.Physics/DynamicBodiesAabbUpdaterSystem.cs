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

        internal DynamicBodiesAabbUpdaterSystem(
            IWorld world,
            IShapeMan shapeMan) :
            base(world)
        {
            this.shapeMan = shapeMan;

            RequireEntityWith<BodyComponent>();
            RequireEntityWith<PositionComponent>();
            RequireEntityWith<VelocityComponent>();

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

        protected override void OnAddEntity(IEntity entity)
        {
            base.OnAddEntity(entity);

            var aabb = Calculate(entity);
            broadphaseDynamic.InsertItem(entity.Id, aabb);
        }

        protected override void OnRemoveEntity(IEntity entity)
        {
            base.OnRemoveEntity(entity);

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
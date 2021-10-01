using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class DynamicBodiesAabbUpdaterSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IFixtureMan fixtureMan;
        private IBroadphaseDynamic broadphaseDynamic;

        #endregion Private Fields

        #region Internal Constructors

        internal DynamicBodiesAabbUpdaterSystem(IFixtureMan fixtureMan)
        {
            this.fixtureMan = fixtureMan;

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

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];

                var aabb = Calculate(entity);

                broadphaseDynamic.UpdateItem(entity.Id, aabb);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);

            var aabb = Calculate(entity);

            broadphaseDynamic.InsertItem(entity.Id, aabb);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);

            broadphaseDynamic.RemoveItem(entity.Id);
        }

        #endregion Protected Methods

        #region Private Methods

        private Box2 Calculate(Entity entity)
        {
            var body = entity.Get<BodyComponent>();
            var pos = entity.Get<PositionComponent>();
            var fixture = fixtureMan.GetById(body.Fixtures.First());
            return fixture.Shape.GetAabb().Translated(pos.Value);
        }

        #endregion Private Methods
    }
}
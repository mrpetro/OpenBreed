using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Commands;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class CollisionResponseSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IEntityMan entityMan;
        private readonly IFixtureMan fixtureMan;
        private readonly IWorldMan worldMan;
        private readonly ICollisionMan collisionMan;

        #endregion Private Fields

        #region Internal Constructors

        public CollisionResponseSystem(IEntityMan entityMan, IFixtureMan fixtureMan, IWorldMan worldMan, ICollisionMan collisionMan)
        {
            this.entityMan = entityMan;
            this.fixtureMan = fixtureMan;
            this.worldMan = worldMan;
            this.collisionMan = collisionMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void UpdatePauseImmuneOnly(float dt)
        {

        }

        private IEnumerable<Entity> GetEntitiesWith<TComponent>()
        {
            var world = worldMan.GetById(WorldId);
            return world.Entities.Where(entity => entity.Contains<TComponent>());
        }

        public void Update(float dt)
        {
            var entities = GetEntitiesWith<CollisionComponent>();

            foreach (var entity in entities)
            {
                var collisionComponent = entity.Get<CollisionComponent>();

                foreach (var contact in collisionComponent.Contacts)
                {
                    var contactEntity = entityMan.GetById(contact.EntityId);

                    collisionMan.Callback(entity, contactEntity, contact.Projection);
                }

                entity.Remove<CollisionComponent>();
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods



        #endregion Private Methods
    }
}
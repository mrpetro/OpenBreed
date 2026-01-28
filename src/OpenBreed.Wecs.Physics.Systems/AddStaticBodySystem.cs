using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Physics.Components;
using OpenBreed.Wecs.Core.Systems.Categories;
using OpenBreed.Wecs.Physics.Systems.Extensions;

namespace OpenBreed.Wecs.Physics.Systems
{
    [RequireEntityWith(
        typeof(CollisionComponent))]
    [SystemCategory(CommonCategories.Physics)]
    public class AddStaticBodySystem : IEventSystem<EntityEnteredEvent>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public AddStaticBodySystem(
            IEntityMan entityMan,
            IWorldMan worldMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(EntityEnteredEvent e)
        {
            var world = this.worldMan.GetById(e.WorldId);

            var entities = world.GetMatchingEntities(this);

            var eventEntity = entityMan.GetById(e.EntityId);

            //Check if cell entity has static body
            if (!(eventEntity.Contains<PositionComponent>() &&
                eventEntity.Contains<BodyComponent>() &&
                !eventEntity.Contains<VelocityComponent>()))
            {
                return;
            }

            var eventWorld = worldMan.GetById(e.WorldId);

            foreach (var entity in entities)
            {
                if (entity.WorldId != eventWorld.Id)
                    continue;

                entity.AddEntityToStatics(eventEntity);
            }
        }

        #endregion Public Methods
    }
}
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
    public class RemoveDynamicBodySystem : IEventSystem<EntityLeftEvent>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public RemoveDynamicBodySystem(
            IEntityMan entityMan,
            IWorldMan worldMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(EntityLeftEvent e)
        {
            var world = this.worldMan.GetById(e.WorldId);

            var entities = world.GetMatchingEntities(this);

            var eventEntity = entityMan.GetById(e.EntityId);

            //Check if cell entity has dynamic body
            if (!eventEntity.Contains<PositionComponent>() ||
                !eventEntity.Contains<BodyComponent>() ||
                !eventEntity.Contains<VelocityComponent>())
            {
                return;
            }

            foreach (var entity in entities)
            {
                if (entity.WorldId != world.Id)
                    continue;

                entity.RemoveEntityFromDynamic(eventEntity);
            }
        }

        #endregion Public Methods
    }
}
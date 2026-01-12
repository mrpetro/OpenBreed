using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Systems.Core.Categories;
using OpenBreed.Wecs.Systems.Physics.Extensions;

namespace OpenBreed.Wecs.Systems.Physics
{
    [RequireEntityWith(
        typeof(CollisionComponent))]
    [SystemCategory(CommonCategories.Physics)]
    public class RemoveDynamicBodySystem : IMatchingSystem, IEventSystem<EntityLeftEvent>
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

        public void Update(EntityLeftEvent e)
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
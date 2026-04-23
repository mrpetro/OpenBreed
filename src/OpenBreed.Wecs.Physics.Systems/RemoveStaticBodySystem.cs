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
    public class RemoveStaticBodySystem : IEventSystem<EntityLeavingEvent>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public RemoveStaticBodySystem(
            IEntityMan entityMan,
            IWorldMan worldMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(IWorld world, EntityLeavingEvent e)
        {
            if (e.WorldId != world.Id)
            {
                return;
            }

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

                entity.RemoveEntityFromStatics(eventEntity);
            }
        }

        #endregion Public Methods

        //protected override void UpdateEntity(IEntity entity, IWorldContext context)
        //{
        //    var entityIds = entity.Get<BroadphaseStaticPutterComponent>().Ids;
        //    var grid = entity.Get<BroadphaseStaticComponent>().Grid;

        //    //Update all tiles
        //    for (int i = 0; i < entityIds.Count; i++)
        //        entity.AddEntityToStatics(entityMan.GetById(entityIds[i]));

        //    entityIds.Clear();
        //}
    }
}
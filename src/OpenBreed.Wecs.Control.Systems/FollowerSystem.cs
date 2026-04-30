using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Abstractions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Control.Systems.Events;
using OpenBreed.Wecs.Core.Systems;
using OpenBreed.Wecs.Core.Systems.Categories;
using OpenBreed.Wecs.Abstractions.Events;

namespace OpenBreed.Wecs.Control.Systems
{
    [SystemCategory(CommonCategories.Control)]
    public class FollowerSystem : IEventSystem<EntityEnteringEvent>
    {
        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public FollowerSystem(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(EntityEnteringEvent e, IWorld world)
        {
            var enteringEntity = entityMan.GetById(e.EntityId);

            var fc = enteringEntity.TryGet<FollowedComponent>();

            if (fc is null)
            {
                return;
            }

            for (int i = 0; i < fc.FollowerIds.Count; i++)
            {
                var follower = entityMan.GetById(fc.FollowerIds[i]);

                if (follower is null)
                {
                    continue;
                }

                //If follower is not in the same world as followed then
                //Make sure it will arrive there
                if (follower.WorldId != e.WorldId)
                {
                    worldMan.RequestAddEntity(follower, e.WorldId);
                    continue;
                }

                RaiseEntityFollowEvent(enteringEntity, follower.Id);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void RaiseEntityFollowEvent(IEntity entity, int followerId)
        {
            eventsMan.Raise(new EntityFollowEvent(entity.Id, followerId));
        }

        #endregion Private Methods
    }
}
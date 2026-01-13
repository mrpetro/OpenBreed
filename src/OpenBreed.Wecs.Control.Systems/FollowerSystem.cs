using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Abstractions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Control.Systems.Events;
using OpenBreed.Wecs.Core.Systems;
using OpenBreed.Wecs.Core.Systems.Categories;

namespace OpenBreed.Wecs.Control.Systems
{
    [RequireEntityWith(
        typeof(FollowedComponent))]
    [SystemCategory(CommonCategories.Control)]
    public class FollowerSystem : UpdatableMatchingSystemBase
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
            IEventsMan eventsMan) : base(worldMan)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var fc = entity.Get<FollowedComponent>();

            for (int i = 0; i < fc.FollowerIds.Count; i++)
            {
                var follower = entityMan.GetById(fc.FollowerIds[i]);

                if (follower is null)
                    continue;

                //If follower is not in the same world as followed then
                //Make sure it will arrive there
                if (follower.WorldId != entity.WorldId)
                {
                    //If follower is in limbo then enter same world as followed
                    //Otherwise follower needs to leave its current world
                    if (follower.WorldId == WecsConsts.NO_WORLD_ID)
                        worldMan.RequestAddEntity(follower, entity.WorldId);
                    else
                        worldMan.RequestRemoveEntity(follower);

                    continue;
                }

                RaiseEntityFollowEvent(entity, follower.Id);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void RaiseEntityFollowEvent(IEntity entity, int followerId)
        {
            eventsMan.Raise(new EntityFollowEvent(entity.Id, followerId));
        }

        #endregion Private Methods
    }
}
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Control
{
    public class FollowerSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Public Constructors

        public FollowerSystem(
            IWorld world,
            IEntityMan entityMan) :
            base(world)
        {
            this.entityMan = entityMan;

            RequireEntityWith<FollowedComponent>();
            RequireEntityWith<PositionComponent>();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var fc = entity.Get<FollowedComponent>();

            for (int i = 0; i < fc.FollowerIds.Count; i++)
            {
                var follower = entityMan.GetById(fc.FollowerIds[i]);

                if (follower == null)
                    continue;

                //If follower is not in the same world as folloed then
                //Make sure it will arrive there
                if (follower.WorldId != entity.WorldId)
                {
                    //If follower is in limbo then enter same world as followed
                    //Otherwise follower needs to leave its current world
                    if (follower.WorldId == WecsConsts.NO_WORLD_ID)
                        follower.EnterWorld(entity.WorldId);
                    else
                        follower.LeaveWorld();

                    continue;
                }

                Glue(entity, follower);
                //Follow(followed, follower);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void Follow(IEntity followed, IEntity follower)
        {
            var followedPos = followed.Get<PositionComponent>();
            var followerPos = follower.Get<PositionComponent>();
            var difference = followedPos.Value - followerPos.Value;
            followerPos.Value += difference / 10;
        }

        private void Glue(IEntity followed, IEntity follower)
        {
            var followedPos = followed.Get<PositionComponent>();
            var followerPos = follower.Get<PositionComponent>();

            followerPos.Value = followedPos.Value;
        }

        #endregion Private Methods
    }
}
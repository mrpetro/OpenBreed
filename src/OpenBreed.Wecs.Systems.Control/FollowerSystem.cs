﻿using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Control
{
    [RequireEntityWith(
        typeof(FollowedComponent),
        typeof(PositionComponent))]
    public class FollowerSystem : UpdatableSystemBase<FollowerSystem>
    {
        private readonly IWorldMan worldMan;
        #region Private Fields

        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Public Constructors

        public FollowerSystem(
            IWorld world,
            IWorldMan worldMan,
            IEntityMan entityMan) :
            base(world)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;

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
                        worldMan.RequestAddEntity(follower, entity.WorldId);
                    else
                        worldMan.RequestRemoveEntity(follower);

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
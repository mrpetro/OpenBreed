using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs;

namespace OpenBreed.Wecs.Systems.Core
{
    public class FollowerSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Public Constructors

        public FollowerSystem(ICore core) : base(core)
        {
            Require<FollowerComponent>();
            Require<PositionComponent>();

        }

        public static void RegisterHandlers(ICommandsMan commands)
        {
            commands.Register<FollowedAddFollowerCommand>(HandleFollowedAddFollowerCommand);
        }

        #endregion Public Constructors

        #region Public Methods

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                Update(entities[i], dt);
        }

        private void Update(Entity entity, float dt)
        {
            var fc = entity.Get<FollowerComponent>();

            for (int i = 0; i < fc.FollowerIds.Count; i++)
            {
                var followerEntity = Core.GetManager<IEntityMan>().GetById(fc.FollowerIds[i]);

                if (followerEntity == null)
                    continue;

                //Glue(entity, followerEntity);
                Follow(entity, followerEntity);
            }
        }

        private void Follow(Entity followed, Entity follower)
        {
            var followedPos = followed.Get<PositionComponent>();
            var followerPos = follower.Get<PositionComponent>();
            var difference = followedPos.Value - followerPos.Value;
            followerPos.Value += difference / 10;
        }

        private void Glue(Entity followed, Entity follower)
        {
            var followedPos = followed.Get<PositionComponent>();
            var followerPos = follower.Get<PositionComponent>();

            followerPos.Value = followedPos.Value;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);

            var fc = entity.Get<FollowerComponent>();

            for (int i = 0; i < fc.FollowerIds.Count; i++)
            {
                var follower = Core.GetManager<IEntityMan>().GetById(fc.FollowerIds[i]);

                if (follower == null)
                    continue;

                follower.Core.Commands.Post(new AddEntityCommand(World.Id, follower.Id));
            }
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);

            var fc = entity.Get<FollowerComponent>();

            for (int i = 0; i < fc.FollowerIds.Count; i++)
            {
                var follower = Core.GetManager<IEntityMan>().GetById(fc.FollowerIds[i]);

                if (follower == null)
                    continue;

                follower.Core.Commands.Post(new RemoveEntityCommand(World.Id, follower.Id));
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private static bool HandleFollowedAddFollowerCommand(ICore core, FollowedAddFollowerCommand cmd)
        {
            var entity = core.GetManager<IEntityMan>().GetById(cmd.EntityId);
            var fc = entity.Get<FollowerComponent>();
            fc.FollowerIds.Add(cmd.FollowerEntityId);
            return true;
        }

        #endregion Private Methods
    }
}
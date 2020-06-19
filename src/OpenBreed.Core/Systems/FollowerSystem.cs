using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Core.Systems
{
    public class FollowerSystem : WorldSystem, ICommandExecutor, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();

        private readonly CommandHandler cmdHandler;

        #endregion Private Fields

        #region Public Constructors

        public FollowerSystem(ICore core) : base(core)
        {
            cmdHandler = new CommandHandler(this);
            Require<FollowerComponent>();
            Require<PositionComponent>();

            //Core.Worlds.Subscribe<EntityAddedEventArgs>(OnEntityAddedEventArgs);
        }

        //private void OnEntityAddedEventArgs(object sender, EntityAddedEventArgs e)
        //{
        //    var followeEntities = Core.Entities.Where(item => item.Contains<FollowerComponent>());

        //    var foundFollower = followerEntities.FirstOrDefault(item => item.GetComponent<FollowerComponent>().FollowedEntityId == e.EntityId);

        //    if (foundFollower == null)
        //        return;

        //    Core.Commands.Post(new AddEntityCommand(e.WorldId, foundFollower.Id));
        //}

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.RegisterHandler(FollowedAddFollowerCommand.TYPE, cmdHandler);
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            cmdHandler.ExecuteEnqueued();
        }

        public void Update(float dt)
        {
            cmdHandler.ExecuteEnqueued();

            for (int i = 0; i < entities.Count; i++)
                Update(entities[i], dt);
        }

        private void Update(Entity entity, float dt)
        {
            var fc = entity.Get<FollowerComponent>();

            for (int i = 0; i < fc.FollowerIds.Count; i++)
            {
                var followerEntity = Core.Entities.GetById(fc.FollowerIds[i]);

                if (followerEntity == null)
                    continue;

                //Glue(entity, followerEntity);
                Follow(entity, followerEntity);
            }
        }

        private void Follow (Entity followed, Entity follower)
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

        public override bool ExecuteCommand(ICommand cmd)
        {
            switch (cmd.Type)
            {
                case FollowedAddFollowerCommand.TYPE:
                    return HandleFollowedAddFollowerCommand((FollowedAddFollowerCommand)cmd);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);

            var fc = entity.Get<FollowerComponent>();

            for (int i = 0; i < fc.FollowerIds.Count; i++)
            {
                var follower = Core.Entities.GetById(fc.FollowerIds[i]);

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
                var follower = Core.Entities.GetById(fc.FollowerIds[i]);

                if (follower == null)
                    continue;

                follower.Core.Commands.Post(new RemoveEntityCommand(World.Id, follower.Id));
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleFollowedAddFollowerCommand(FollowedAddFollowerCommand cmd)
        {
            var entity = Core.Entities.GetById(cmd.EntityId);
            var fc = entity.Get<FollowerComponent>();
            fc.FollowerIds.Add(cmd.FollowerEntityId);
            return true;
        }

        #endregion Private Methods
    }
}
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

        private readonly List<IEntity> entities = new List<IEntity>();

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
        //    var followerEntities = Core.Entities.Where(item => item.Contains<FollowerComponent>());

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

            World.RegisterHandler(FollowerSetTargetCommand.TYPE, cmdHandler);
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

        private void Update(IEntity entity, float dt)
        {
            var fc = entity.GetComponent<FollowerComponent>();

            var target = Core.Entities.GetById(fc.FollowedEntityId);

            if (target == null)
                return;

            var targetPos = target.GetComponent<PositionComponent>();
            var followerPos = entity.GetComponent<PositionComponent>();

            //Just follower position to it's target position
            followerPos.Value = targetPos.Value;
        }

        public override bool ExecuteCommand(ICommand cmd)
        {
            switch (cmd.Type)
            {
                case FollowerSetTargetCommand.TYPE:
                    return HandleFollowerSetTargetCommand((FollowerSetTargetCommand)cmd);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleFollowerSetTargetCommand(FollowerSetTargetCommand cmd)
        {
            var entity = Core.Entities.GetById(cmd.EntityId);
            var fc = entity.GetComponent<FollowerComponent>();
            fc.FollowedEntityId = cmd.TargetEntityId;
            return true;
        }

        #endregion Private Methods
    }
}
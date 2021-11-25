using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Control
{
    public class FollowerSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Public Constructors

        public FollowerSystem(IEntityMan entityMan)
        {
            this.entityMan = entityMan;

            RequireEntityWith<FollowerComponent>();
            RequireEntityWith<PositionComponent>();
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

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void Update(Entity followed, float dt)
        {
            var fc = followed.Get<FollowerComponent>();

            for (int i = 0; i < fc.FollowerIds.Count; i++)
            {
                var follower = entityMan.GetById(fc.FollowerIds[i]);

                if (follower == null)
                    continue;

                //If follower is not in the same world as folloed then
                //Make sure it will arrive there
                if (follower.WorldId != followed.WorldId)
                {
                    //If follower is in limbo then enter same world as followed
                    //Otherwise follower needs to leave its current world
                    if(follower.WorldId == World.NO_WORLD)
                        follower.EnterWorld(followed.WorldId);
                    else
                        follower.LeaveWorld();

                    continue;
                }

                Glue(followed, follower);
                //Follow(followed, follower);
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

        #endregion Private Methods
    }
}
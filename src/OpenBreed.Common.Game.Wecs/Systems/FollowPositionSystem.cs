using OpenBreed.Common.Game.Services;
using OpenBreed.Core.Abstractions.Events;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Physics.Systems.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    public class FollowPositionSystem : IEventSystem<PositionChangedEvent>, IEventSystem<EntityEnteredEvent>
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public FollowPositionSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(PositionChangedEvent e)
        {
            TryFollow(e.EntityId);
        }

        public void OnEvent(EntityEnteredEvent e)
        {
            var entity = services.Entities.GetById(e.EntityId);

            var followedComponent = entity.TryGet<FollowedComponent>();

            if (followedComponent is null)
            {
                return;
            }

            TryFollow(e.EntityId);
        }

        #endregion Public Methods

        #region Private Methods

        private void TryFollow(int followedId)
        {
            var entity = services.Entities.GetById(followedId);

            var followedComponent = entity.TryGet<FollowedComponent>();

            if (followedComponent is null)
            {
                return;
            }

            var followedPos = entity.Get<PositionComponent>();

            foreach (var followerId in followedComponent.FollowerIds)
            {
                var follower = services.Entities.GetById(followerId);
                var followerPos = follower.Get<PositionComponent>();
                followerPos.Value = followedPos.Value;
            }
        }

        #endregion Private Methods
    }
}
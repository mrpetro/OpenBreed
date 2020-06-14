using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Common.Components
{
    public class FollowerComponent : IEntityComponent
    {
        #region Public Properties

        public FollowerComponent(int followedEntityId = -1)
        {
            FollowedEntityId = followedEntityId;
        }

        public int FollowedEntityId { get; set; }

        #endregion Public Properties
    }
}
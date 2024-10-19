using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IFollowedComponentTemplate : IComponentTemplate
    {
    }

    /// <summary>
    /// Component used by Follower system to store followers
    /// of Entity that has this component
    /// </summary>
    [ComponentName("Followed")]
    public class FollowedComponent : IEntityComponent
    {
        #region Public Constructors

        public FollowedComponent()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public List<int> FollowerIds { get; } = new List<int>();

        #endregion Public Properties
    }

    public sealed class FollowedComponentFactory : ComponentFactoryBase<IFollowedComponentTemplate>
    {
        #region Internal Constructors

        public FollowedComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IFollowedComponentTemplate template)
        {
            return new FollowedComponent();
        }

        #endregion Protected Methods
    }

}
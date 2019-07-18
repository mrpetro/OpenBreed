using System;

namespace OpenBreed.Core.Common.Systems.Components
{
    public class GroupPart : IEntityComponent
    {
        #region Public Constructors

        public GroupPart(int entityId)
        {
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Grouping Entity ID
        /// </summary>
        public int EntityId { get; }

        #endregion Public Properties
    }
}
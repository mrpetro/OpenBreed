using System;

namespace OpenBreed.Core.Common.Systems.Components
{
    public class GroupPart : IEntityComponent
    {
        #region Public Constructors

        public GroupPart(Guid guid)
        {
            Guid = guid;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Grouping Entity Guid
        /// </summary>
        public Guid Guid { get; }

        #endregion Public Properties
    }
}
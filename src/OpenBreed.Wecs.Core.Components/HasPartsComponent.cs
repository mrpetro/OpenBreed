using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBreed.Wecs.Core.Components
{
    [ComponentName("IsPartOf")]
    public class IsPartOfComponent : IEntityComponent
    {
        public IsPartOfComponent()
        {
        }

        public int EntityId { get; set; }
    }

    [ComponentName("HasParts")]
    public class HasPartsComponent : IEntityComponent
    {
        #region Private Fields

        private readonly List<int> entityIds = new List<int>();

        #endregion Private Fields

        #region Public Constructors

        public HasPartsComponent()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public IReadOnlyCollection<int> EntityIds => entityIds;

        #endregion Public Properties

        #region Public Methods

        public void Add(int entityId)
        {
            entityIds.Add(entityId);
        }

        #endregion Public Methods
    }
}
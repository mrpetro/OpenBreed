using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBreed.Wecs.Core.Components
{
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
    }
}
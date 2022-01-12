using OpenBreed.Wecs.Components;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Common
{
    public class GroupComponentEx : IEntityComponent
    {
        #region Public Constructors

        public GroupComponentEx(int id)
        {
            Id = id;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Ids of entities which are members of this group 
        /// </summary>
        public int Id { get; }

        #endregion Public Properties
    }
}
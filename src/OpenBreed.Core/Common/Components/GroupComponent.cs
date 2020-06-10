﻿using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Systems.Components;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Components
{
    public class GroupComponent : IEntityComponent
    {
        #region Public Constructors

        public GroupComponent()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Ids of entities which are members of this group 
        /// </summary>
        public List<int> MemberIds { get; } = new List<int>();

        #endregion Public Properties
    }
}
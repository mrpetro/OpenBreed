﻿using OpenBreed.Core.Common.Systems.Components;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Components
{
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
}
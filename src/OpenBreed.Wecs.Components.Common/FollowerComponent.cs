﻿using OpenBreed.Wecs.Components;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Common
{
    public class FollowerComponent : IEntityComponent
    {
        #region Public Constructors

        public FollowerComponent()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public List<int> FollowerIds { get; } = new List<int>();

        #endregion Public Properties
    }
}
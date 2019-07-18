﻿using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Physics.Components
{
    /// <summary>
    /// Physical body component interface which is used in entites that are moving in world coordinates
    /// </summary>
    public interface IDynamicBody : IPhysicsComponent
    {
        #region Public Properties

        /// <summary>
        /// Non real physical factor for friction.
        /// </summary>
        float FrictionFactor { get; }

        /// <summary>
        /// Position from previous frame used for Verlet
        /// </summary>
        Vector2 OldPosition { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        bool Collides { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        List<Tuple<int, int>> Boxes { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        Vector2 Projection { get; set; }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods
    }
}
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Physics.Components
{
    /// <summary>
    /// Body component data used for physical interactions in world
    /// </summary>
    public interface IBody : IPhysicsComponent
    {
        #region Public Properties

        /// <summary>
        /// Coefficient of friction factor for this body.
        /// </summary>
        float CofFactor { get; }

        /// <summary>
        /// Coefficient of restitution factor for this body.
        /// </summary>
        float CorFactor { get; }

        /// <summary>
        /// User defined tag
        /// </summary>
        string Tag { get; set; }

        /// <summary>
        /// Position from previous frame used for Verlet
        /// </summary>
        Vector2 OldPosition { get; set; }

        /// <summary>
        /// Collistion callback
        /// </summary>
        Action<IEntity, Vector2> CollisionCallback { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        List<Tuple<int, int>> Boxes { get; set; }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods
    }
}
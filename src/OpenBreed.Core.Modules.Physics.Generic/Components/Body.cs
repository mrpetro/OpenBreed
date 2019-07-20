using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Core.Modules.Physics.Systems;
using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Modules.Physics.Components
{
    /// <summary>
    /// Physical Body data
    /// </summary>
    public class Body : IBody
    {
        /// <summary>
        /// Coefficient of friction factor for this body.
        /// </summary>
        public float CofFactor { get; internal set; }

        /// <summary>
        /// Coefficient of restitution factor for this body.
        /// </summary>
        public float CorFactor { get; internal set; }

        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public Body(float cofFactor, float corFactor)
        {
            CofFactor = cofFactor;
            CorFactor = corFactor;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// DEBUG only
        /// </summary>
        public bool Collides { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        ///
        public List<Tuple<int, int>> Boxes { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        ///
        public Vector2 Projection { get;  set; }

        /// <summary>
        /// OldPosition used for verlet integration
        /// </summary>
        public Vector2 OldPosition { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {
        }

        #endregion Public Methods

        #region Private Methods


        #endregion Private Methods
    }
}
using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Physics.Components
{
    /// <summary>
    /// Physical Body data
    /// </summary>
    public class BodyComponent : IEntityComponent
    {
        #region Public Constructors

        public BodyComponent()
        {
        }

        #endregion Public Constructors

        #region Private Constructors

        private BodyComponent(float cofFactor, float corFactor, string tag)
        {
            CofFactor = cofFactor;
            CorFactor = corFactor;
            Tag = tag;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Coefficient of friction factor for this body.
        /// </summary>
        public float CofFactor { get; internal set; }

        /// <summary>
        /// Coefficient of restitution factor for this body.
        /// </summary>
        public float CorFactor { get; internal set; }

        /// <summary>
        /// List of Fixture IDs
        /// </summary>
        public List<int> Fixtures { get; internal set; }

        /// <summary>
        /// User defined tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        ///
        public List<Tuple<int, int>> Boxes { get; set; }

        /// <summary>
        /// DEBUG only
        /// </summary>
        ///
        public Vector2 Projection { get; set; }

        /// <summary>
        /// OldPosition used for verlet integration
        /// </summary>
        public Vector2 OldPosition { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static BodyComponent Create(float cofFactor, float corFactor, string tag)
        {
            return new BodyComponent(cofFactor, corFactor, tag);
        }

        #endregion Public Methods
    }
}
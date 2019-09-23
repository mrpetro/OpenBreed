using OpenBreed.Core.Entities;
using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Physics.Components
{
    /// <summary>
    /// Physical Body data
    /// </summary>
    public class Body : IBody
    {
        #region Public Constructors

        public Body()
        {
        }

        #endregion Public Constructors

        #region Private Constructors

        private Body(float cofFactor, float corFactor, string tag, Action<IEntity, Vector2> collisionCallback)
        {
            CofFactor = cofFactor;
            CorFactor = corFactor;
            Tag = tag;
            CollisionCallback = collisionCallback;
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

        /// <summary>
        /// Collistion callback
        /// </summary>
        public Action<IEntity, Vector2> CollisionCallback { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static Body Create(float cofFactor, float corFactor, string tag, Action<IEntity, Vector2> collisionCallback = null)
        {
            return new Body(cofFactor, corFactor, tag, collisionCallback);
        }

        #endregion Public Methods
    }
}
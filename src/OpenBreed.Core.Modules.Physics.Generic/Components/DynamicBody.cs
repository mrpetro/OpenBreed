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
    /// Dynamic body
    /// Borrowing calculations from amazing N tutorials implementation for now:
    /// https://www.metanetsoftware.com/technique/tutorialA.html
    /// </summary>
    public class DynamicBody : IDynamicBody
    {
        /// <summary>
        /// Non real physical factor for friction.
        /// </summary>
        public float FrictionFactor { get; internal set; }


        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public DynamicBody(float frictionFactor)
        {
            FrictionFactor = frictionFactor;
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

        public Box2 Aabb
        {
            get
            {
                return new Box2();
            }

            set
            {

            }
        }

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
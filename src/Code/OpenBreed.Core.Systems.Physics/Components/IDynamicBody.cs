using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Systems.Physics.Components
{
    /// <summary>
    /// Physical body component interface which is used in entites that are moving in world coordinates
    /// </summary>
    public interface IDynamicBody : IPhysicsComponent
    {
        #region Public Properties

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
        Vector2 Projection { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Check/Report collision with static body
        /// </summary>
        /// <param name="staticBody">Static body to check and report collision</param>
        void CollideVsStatic(IStaticBody staticBody);

        /// <summary>
        /// Check/Report collision with dynamic body
        /// </summary>
        /// <param name="dynamicBody">Dynamic body to check and report collision</param>
        void CollideVsDynamic(IDynamicBody dynamicBody);

        void IntegrateVerlet();

        #endregion Public Methods
    }
}
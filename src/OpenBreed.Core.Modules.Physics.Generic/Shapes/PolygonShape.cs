using OpenBreed.Core.Managers;
using OpenTK;
using System;

namespace OpenBreed.Core.Modules.Physics.Shapes
{
    /// <summary>
    /// Place holder implementation for fixture polygon shape
    /// </summary>
    public class PolygonShape : IShape
    {
        #region Public Properties

        public Vector2[] Vertices { get; }

        #endregion Public Properties

        #region Public Methods

        public Box2 GetAabb()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
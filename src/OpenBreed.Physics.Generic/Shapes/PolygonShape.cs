using OpenBreed.Physics.Interface;
using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Physics.Generic.Shapes
{
    /// <summary>
    /// Place holder implementation for fixture polygon shape
    /// </summary>
    public class PolygonShape : IPolygonShape
    {
        #region Public Properties

        public Vector2[] Vertices { get; }

        #endregion Public Properties

        #region Public Methods

        public Box2 GetAabb()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Polygon (Vertices: {Vertices.Length})";
        }

        #endregion Public Methods
    }
}
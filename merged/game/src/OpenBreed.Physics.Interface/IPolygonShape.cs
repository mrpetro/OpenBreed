using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Physics.Interface
{
    /// <summary>
    /// Polygon shape interface for fixtures
    /// </summary>
    public interface IPolygonShape : IShape
    {
        #region Public Properties

        Vector2[] Vertices { get; }

        #endregion Public Properties
    }
}
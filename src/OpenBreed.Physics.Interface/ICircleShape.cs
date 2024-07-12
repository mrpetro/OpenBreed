using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Physics.Interface
{
    /// <summary>
    /// Circle shape interface for fixtures
    /// </summary>
    public interface ICircleShape : IShape
    {
        #region Public Properties

        Vector2 Center { get; }
        float Radius { get; }

        #endregion Public Properties
    }
}
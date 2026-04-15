using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Physics.Interface
{
    /// <summary>
    /// Box shape interface for fixtures
    /// </summary>
    public interface IBoxShape : IShape
    {
        #region Public Properties

        float Height { get; }
        float Width { get; }
        float X { get; }
        float Y { get; }

        #endregion Public Properties
    }
}
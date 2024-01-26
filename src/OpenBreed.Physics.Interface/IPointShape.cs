using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Physics.Interface
{
    /// <summary>
    /// Point shape interface for fixtures
    /// </summary>
    public interface IPointShape : IShape
    {
        #region Public Properties

        float X { get; }
        float Y { get; }

        #endregion Public Properties
    }
}
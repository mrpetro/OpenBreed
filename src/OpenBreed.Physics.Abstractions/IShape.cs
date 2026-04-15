using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Physics.Interface
{
    /// <summary>
    /// Shape interface for fixtures
    /// </summary>
    public interface IShape
    {
        #region Public Methods

        Box2 GetAabb();

        #endregion Public Methods
    }
}
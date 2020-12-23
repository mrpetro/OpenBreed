using OpenTK;

namespace OpenBreed.Core.Managers
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
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Interface
{
    /// <summary>
    /// Interface for accessing sprite atlas
    /// </summary>
    public interface ISpriteAtlas
    {
        #region Public Properties

        /// <summary>
        /// Id of this sprite atlas
        /// </summary>
        int Id { get; }

        #endregion Public Properties

        #region Public Methods

        Vector2 GetSpriteSize(int spriteId);

        bool IsValid(int imageId);

        #endregion Public Methods
    }
}
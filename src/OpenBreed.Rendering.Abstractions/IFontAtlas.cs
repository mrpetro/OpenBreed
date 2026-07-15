
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Abstractions
{
    /// <summary>
    /// Interface for accessing font atlas
    /// </summary>
    public interface IFontAtlas
    {
        #region Public Properties

        /// <summary>
        /// Id of this font
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Get font height
        /// </summary>
        float Height { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Draw text given in parameter
        /// </summary>
        /// <param name="text">Text to draw</param>
        /// <param name="color">Color of text</param>
        /// <param name="clipBox">Clip box</param>
        void Draw(IRenderView view, string text, Color4<Rgba> color, Box2 clipBox, bool ignoreScale = false);

        /// <summary>
        /// Gets single character width
        /// </summary>
        /// <param name="character">Single character to get width from</param>
        /// <returns>Character width value</returns>
        float GetWidth(char character);

        /// <summary>
        /// Gets text width
        /// </summary>
        /// <param name="text">Text to get width from</param>
        /// <returns>Text width value</returns>
        float GetWidth(string text);

        #endregion Public Methods
    }
}
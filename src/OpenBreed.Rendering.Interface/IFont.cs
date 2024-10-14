
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Interface
{
    /// <summary>
    /// Interface for accessing font functionality
    /// </summary>
    public interface IFont
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

        void Render(IRenderView view, string text, Box2 clipBox, Vector2 pos, float order);

        /// <summary>
        /// Draw single character given in parameter
        /// </summary>
        /// <param name="character">Single character to draw</param>
        void Draw(IRenderView view, char character, Box2 clipBox, bool ignoreScale = false);

        /// <summary>
        /// Draw text given in parameter
        /// </summary>
        /// <param name="text">Text to draw</param>
        /// <param name="color">Color of text</param>
        /// <param name="clipBox">Clip box</param>
        void Draw(IRenderView view, string text, Color4 color, Box2 clipBox, bool ignoreScale = false);

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
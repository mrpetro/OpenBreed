namespace OpenBreed.Core.Modules.Rendering.Helpers
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

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Draw text given in parameter
        /// </summary>
        /// <param name="text">Text to draw</param>
        void Draw(string text);

        /// <summary>
        /// Gets single character width
        /// </summary>
        /// <param name="character">Character to get width from</param>
        /// <returns>Width value</returns>
        float GetWidth(char character);

        /// <summary>
        /// Gets text width
        /// </summary>
        /// <param name="text">Text to get width from</param>
        /// <returns>Width value</returns>
        float GetWidth(string text);

        #endregion Public Methods
    }
}
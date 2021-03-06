﻿namespace OpenBreed.Rendering.Interface
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

        /// <summary>
        /// Draw single character given in parameter
        /// </summary>
        /// <param name="character">Single character to draw</param>
        void Draw(char character);

        /// <summary>
        /// Draw text given in parameter
        /// </summary>
        /// <param name="text">Text to draw</param>
        void Draw(string text);

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
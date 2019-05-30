namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// Interface for accessing font functionality
    /// </summary>
    public interface IFont
    {
        #region Public Methods

        /// <summary>
        /// Draw text given in parameter
        /// </summary>
        /// <param name="text">Text to draw</param>
        void Draw(string text);

        #endregion Public Methods
    }
}
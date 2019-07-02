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

        #endregion Public Methods
    }
}
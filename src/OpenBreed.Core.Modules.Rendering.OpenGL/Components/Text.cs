using OpenTK;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    public class Text : IText
    {
        #region Private Constructors

        private Text(int fontId, Vector2 offset, string value)
        {
            FontId = fontId;
            Offset = offset;
            Value = value;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Id of text font
        /// </summary>
        public int FontId { get; set; }

        /// <summary>
        /// Offset position of text
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Actual text of this component
        /// </summary>
        public string Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates text component using given font
        /// </summary>
        /// <param name="fontId">Id of font to use for this text component</param>
        /// <param name="offset">Offset position from position component</param>
        /// <param name="value">Optional initial text value</param>
        /// <returns>Text component</returns>
        public static Text Create(int fontId, Vector2 offset, string value = null)
        {
            return new Text(fontId, offset, value);
        }

        #endregion Public Methods
    }
}
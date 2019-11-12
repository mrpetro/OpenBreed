using OpenTK;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    public class TextComponent : ITextComponent
    {
        #region Private Constructors

        private TextComponent(int fontId, Vector2 offset, string value, float order)
        {
            FontId = fontId;
            Offset = offset;
            Value = value;
            Order = order;
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

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        public float Order { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates text component using given font
        /// </summary>
        /// <param name="fontId">Id of font to use for this text component</param>
        /// <param name="offset">Offset position from position component</param>
        /// <param name="value">Optional initial text value</param>
        /// <param name="order">Optional initial object rendering order</param>
        /// <returns>Text component</returns>
        public static TextComponent Create(int fontId, Vector2 offset, string value = null, float order = 0.0f)
        {
            return new TextComponent(fontId, offset, value, order);
        }

        #endregion Public Methods
    }
}
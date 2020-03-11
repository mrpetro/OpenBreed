using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    public class TextPart
    {
        public TextPart(int fontId, Vector2 offset, Color4 color, string text, float order)
        {
            FontId = fontId;
            Offset = offset;
            Text = text;
            Order = order;
            Color = color;
        }

        /// <summary>
        /// Id of text font
        /// </summary>
        public int FontId { get; set; }

        /// <summary>
        /// Offset position of this text part
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Color of this text part
        /// </summary>
        public Color4 Color { get; set; }

        /// <summary>
        /// Actual text of this part
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        public float Order { get; set; }
    }

    public class TextComponent : IEntityComponent
    {
        #region Private Constructors

        public List<TextPart> Parts { get; }

        private TextComponent(int fontId, Vector2 offset, Color4 color, string value, float order)
        {
            Parts = new List<TextPart>();
            Parts.Add(new TextPart(fontId, offset, color, value, order));
        }

        #endregion Private Constructors

        #region Public Properties

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
        public static TextComponent Create(int fontId, Vector2 offset, Color4 color, string value = null, float order = 0.0f)
        {
            return new TextComponent(fontId, offset, color, value, order);
        }

        #endregion Public Methods
    }
}

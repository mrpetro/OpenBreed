using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Components
{

    public class TextPart
    {
        #region Public Constructors

        public TextPart(int fontId, Vector2 offset, Color4 color, string text, float order)
        {
            FontId = fontId;
            Offset = offset;
            Text = text;
            Order = order;
            Color = color;
        }

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties
    }

    public class TextComponent : IEntityComponent
    {
        #region Internal Constructors

        internal TextComponent(TextComponentBuilder builder)
        {
            Parts = new List<TextPart>();
            Parts.Add(new TextPart(builder.FontId, builder.Offset, builder.Color, builder.Text, builder.Order));
        }

        #endregion Internal Constructors

        #region Public Properties

        public List<TextPart> Parts { get; }

        #endregion Public Properties
    }
}
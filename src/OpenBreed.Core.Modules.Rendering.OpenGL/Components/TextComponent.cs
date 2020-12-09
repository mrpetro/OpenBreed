using OpenBreed.Core.Components;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    public interface ITextComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        /// <summary>
        /// Name of text font
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Size of text font
        /// </summary>
        int FontSize { get; set; }

        /// <summary>
        /// Offset position of this text part
        /// </summary>
        Vector2 Offset { get; set; }

        /// <summary>
        /// Color of this text part
        /// </summary>
        Color4 Color { get; set; }

        /// <summary>
        /// Actual text of this part
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        int Order { get; set; }

        #endregion Public Properties
    }

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

        internal TextComponent(TextComponentBuilderEx builder)
        {
            Parts = new List<TextPart>();
            Parts.Add(new TextPart(builder.FontId, builder.Offset, builder.Color, builder.Text, builder.Order));
        }

        #endregion Internal Constructors

        #region Public Properties

        public List<TextPart> Parts { get; }

        #endregion Public Properties
    }

    public sealed class TextComponentFactory : ComponentFactoryBase<ITextComponentTemplate>
    {
        #region Public Constructors

        public TextComponentFactory(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ITextComponentTemplate template)
        {
            var builder = TextComponentBuilderEx.New(core);
            builder.SetColor(template.Color);
            builder.SetFont(template.FontName, template.FontSize);
            builder.SetOffset(template.Offset);
            builder.SetOrder(template.Order);
            builder.SetText(template.Text);

            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class TextComponentBuilderEx
    {
        #region Private Fields

        private ICore core;

        #endregion Private Fields

        #region Private Constructors

        private TextComponentBuilderEx(ICore core)
        {
            this.core = core;
        }

        #endregion Private Constructors

        #region Internal Properties

        internal int FontId { get; private set; }
        internal Vector2 Offset { get; private set; }
        internal Color4 Color { get; private set; }
        internal string Text { get; private set; }
        internal int Order { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public static TextComponentBuilderEx New(ICore core)
        {
            return new TextComponentBuilderEx(core);
        }

        public TextComponent Build()
        {
            return new TextComponent(this);
        }

        public void SetOrder(int order)
        {
            Order = order;
        }

        public void SetColor(Color4 color)
        {
            Color = color;
        }

        public void SetFontById(int fontId)
        {
            FontId = fontId;
        }

        public void SetFont(string fontName, int fontSize)
        {
            var font = core.Rendering.Fonts.Create(fontName, fontSize);
            SetFontById(font.Id);
        }

        public void SetOffset(Vector2 offset)
        {
            Offset = offset;
        }

        public void SetText(string text)
        {
            Text = text;
        }

        #endregion Public Methods
    }
}
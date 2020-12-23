using OpenBreed.Core.Components.Xml;
using OpenTK;
using OpenTK.Graphics;
using System.Xml.Serialization;

namespace OpenBreed.Rendering.Components.Xml
{
    [XmlRoot("Text")]
    public class XmlTextComponent : XmlComponentTemplate, ITextComponentTemplate
    {
        #region Public Properties

        public string FontName { get; set; }
        public int FontSize { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }

        [XmlIgnore]
        public Vector2 Offset
        {
            get => new Vector2(XmlOffset.X, XmlOffset.Y);
            set => XmlOffset = new XmlVector2() { X = value.X, Y = value.Y };
        }

        [XmlIgnore]
        public Color4 Color
        {
            get => new Color4(XmlColor.R, XmlColor.G, XmlColor.B, XmlColor.A);
            set => XmlColor = new XmlColor4() { R = value.R, G = value.G, B = value.B, A = value.A };
        }

        [XmlElement("Offset")]
        public XmlVector2 XmlOffset { get; set; }

        [XmlElement("Color")]
        public XmlColor4 XmlColor { get; set; }

        #endregion Public Properties
    }
}
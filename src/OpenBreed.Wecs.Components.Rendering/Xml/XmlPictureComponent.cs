using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenTK;
using OpenTK.Mathematics;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Rendering.Xml
{
    [XmlRoot("Picture")]
    public class XmlPictureComponent : XmlComponentTemplate, IPictureComponentTemplate
    {
        #region Public Properties

        [XmlElement("ImageName")]
        public string ImageName { get; set; }

        [XmlIgnore]
        public Vector2 Origin
        {
            get => new Vector2(XmlOrigin.X, XmlOrigin.Y);
            set => XmlOrigin = new XmlVector2() { X = value.X, Y = value.Y };
        }

        [XmlIgnore]
        public Color4 Color
        {
            get => new Color4(XmlColor.R, XmlColor.G, XmlColor.B, XmlColor.A);
            set => XmlColor = new XmlColor4() { R = value.R, G = value.G, B = value.B, A = value.A };
        }

        [XmlElement("Origin")]
        public XmlVector2 XmlOrigin { get; set; }

        [XmlElement("Color")]
        public XmlColor4 XmlColor { get; set; }

        public int Order { get; set; }

        #endregion Public Properties
    }
}
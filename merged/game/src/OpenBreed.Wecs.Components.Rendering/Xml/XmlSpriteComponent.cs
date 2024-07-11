using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenTK.Mathematics;
using System.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Rendering.Xml
{
    [XmlRoot("Sprite")]
    public class XmlSpriteComponent : XmlComponentTemplate, ISpriteComponentTemplate
    {
        #region Public Constructors

        public XmlSpriteComponent()
        {
            XmlScale = new XmlVector2 { X = 1.0f, Y = 1.0f };
        }

        #endregion Public Constructors

        #region Public Properties

        [XmlElement("AtlasName")]
        public string AtlasName { get; set; }

        public bool Hidden { get; set; }

        [XmlElement("ImageIndex")]
        public int ImageIndex { get; set; }

        public int Order { get; set; }

        [XmlIgnore]
        public Vector2 Origin
        {
            get => new Vector2(XmlOrigin.X, XmlOrigin.Y);
            set => XmlOrigin = new XmlVector2() { X = value.X, Y = value.Y };
        }

        [XmlIgnore]
        public Vector2 Scale
        {
            get => new Vector2(XmlScale.X, XmlScale.Y);
            set => XmlScale = new XmlVector2() { X = value.X, Y = value.Y };
        }

        [XmlElement("Origin")]
        public XmlVector2 XmlOrigin { get; set; }

        [XmlElement("Scale")]
        public XmlVector2 XmlScale { get; set; }

        #endregion Public Properties
    }
}
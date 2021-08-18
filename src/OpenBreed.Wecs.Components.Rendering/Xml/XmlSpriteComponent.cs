using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenTK;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Rendering.Xml
{
    [XmlRoot("Sprite")]
    public class XmlSpriteComponent : XmlComponentTemplate, ISpriteComponentTemplate
    {
        #region Public Properties

        public string AtlasName { get; set; }
        public int ImageIndex { get; set; }

        [XmlIgnore]
        public Vector2 Origin
        {
            get => new Vector2(XmlOrigin.X, XmlOrigin.Y);
            set => XmlOrigin = new XmlVector2() { X = value.X, Y = value.Y };
        }

        [XmlElement("Origin")]
        public XmlVector2 XmlOrigin { get; set; }

        public int Order { get; set; }

        #endregion Public Properties
    }
}
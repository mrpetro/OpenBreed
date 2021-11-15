using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenTK;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Rendering.Xml
{
    [XmlRoot("TilePutter")]
    public class XmlTilePutterComponent : XmlComponentTemplate, ITilePutterComponentTemplate
    {
        #region Public Properties

        [XmlElement("AtlasName")]
        public string AtlasName { get; set; }

        [XmlElement("ImageIndex")]
        public int ImageIndex { get; set; }

        [XmlIgnore]
        public Vector2 Position
        {
            get => new Vector2(XmlPosition.X, XmlPosition.Y);
            set => XmlPosition = new XmlVector2() { X = value.X, Y = value.Y };
        }

        [XmlElement("Position")]
        public XmlVector2 XmlPosition { get; set; }

        #endregion Public Properties
    }
}
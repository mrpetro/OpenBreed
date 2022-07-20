using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenTK;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Rendering.Xml
{
    public class XmlTileDataTemplate : ITileDataTemplate
    {
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
    }

    [XmlRoot("TilePutter")]
    public class XmlTilePutterComponent : XmlComponentTemplate, ITilePutterComponentTemplate
    {
        #region Public Properties

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<XmlTileDataTemplate> XmlItems { get; set; }

        [XmlIgnore]
        public IEnumerable<ITileDataTemplate> Items => XmlItems;

        #endregion Public Properties
    }
}
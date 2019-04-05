using OpenBreed.Common.Sprites;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Sprites
{
    public class XmlSpriteCoords : ISpriteCoords
    {
        #region Public Properties

        [XmlAttribute]
        public int Height { get; set; }

        [XmlAttribute]
        public int Width { get; set; }

        [XmlAttribute]
        public int X { get; set; }

        [XmlAttribute]
        public int Y { get; set; }

        #endregion Public Properties
    }
}
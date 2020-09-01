using OpenBreed.Database.Interface.Items.Sprites;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Sprites
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
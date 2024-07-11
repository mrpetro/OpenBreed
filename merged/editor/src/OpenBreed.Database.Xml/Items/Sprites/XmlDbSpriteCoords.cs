using OpenBreed.Database.Interface.Items.Sprites;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Sprites
{
    public class XmlDbSpriteCoords : IDbSpriteCoords
    {
        #region Public Constructors

        public XmlDbSpriteCoords()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbSpriteCoords(XmlDbSpriteCoords other)
        {
            X = other.X;
            Y = other.Y;
            Width = other.Width;
            Height = other.Height;
        }

        #endregion Protected Constructors

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

        #region Public Methods

        public IDbSpriteCoords Copy() => new XmlDbSpriteCoords(this);

        #endregion Public Methods
    }
}
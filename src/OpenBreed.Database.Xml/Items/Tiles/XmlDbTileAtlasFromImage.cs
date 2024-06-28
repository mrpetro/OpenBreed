using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Tiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Tiles
{
    [Serializable]
    [Description("Tile atlas from Image"), Category("Appearance")]
    public class XmlDbTileAtlasFromImage : XmlDbTileAtlas, IDbTileAtlasFromImage
    {
        #region Public Constructors

        public XmlDbTileAtlasFromImage()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbTileAtlasFromImage(XmlDbTileAtlasFromImage other) : base(other)
        {
            DataRef = other.DataRef;
            TileSize = other.TileSize;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlElement("TileSize")]
        public int TileSize { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbTileAtlasFromImage(this);

        #endregion Public Methods
    }
}
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
    [Description("Tile atlas from ACBM"), Category("Appearance")]
    public class XmlDbTileAtlasFromAcbm : XmlDbTileAtlas, IDbTileAtlasFromAcbm
    {
        #region Public Constructors

        public XmlDbTileAtlasFromAcbm()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbTileAtlasFromAcbm(XmlDbTileAtlasFromAcbm other) : base(other)
        {
            DataRef = other.DataRef;
            TileSize = other.TileSize;
            BitPlanesNo = other.BitPlanesNo;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlElement("TileSize")]
        public int TileSize { get; set; }

        [XmlElement("BitPlanesNo")]
        public int BitPlanesNo { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbTileAtlasFromAcbm(this);

        #endregion Public Methods
    }
}
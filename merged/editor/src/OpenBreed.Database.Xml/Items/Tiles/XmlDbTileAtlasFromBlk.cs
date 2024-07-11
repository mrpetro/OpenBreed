using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Xml.Items.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace OpenBreed.Database.Xml.Items.Tiles
{
    [Serializable]
    [Description("Tile atlas from BLK"), Category("Appearance")]
    public class XmlDbTileAtlasFromBlk : XmlDbTileAtlas, IDbTileAtlasFromBlk
    {
        #region Public Constructors

        public XmlDbTileAtlasFromBlk()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbTileAtlasFromBlk(XmlDbTileAtlasFromBlk other) : base(other)
        {
            DataRef = other.DataRef;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbTileAtlasFromBlk(this);

        #endregion Public Methods
    }
}
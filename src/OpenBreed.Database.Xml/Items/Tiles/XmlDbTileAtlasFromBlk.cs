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
    [Description("Tile atlas from BLK"), Category("Appearance")]
    public class XmlDbTileAtlasFromBlk : XmlDbTileAtlas, IDbTileAtlasFromBlk
    {
        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        public override IDbEntry Copy()
        {
            return new XmlDbTileAtlasFromBlk()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef
            };
        }
    }
}

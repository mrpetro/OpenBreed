using OpenBreed.Common.Tiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Tiles
{
    [Serializable]
    [Description("Tile set from BLK"), Category("Appearance")]
    public class XmlTileSetFromBlkEntry : XmlTileSetEntry, ITileSetFromBlkEntry
    {
        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        public override IEntry Copy()
        {
            return new XmlTileSetFromBlkEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef
            };
        }
    }
}

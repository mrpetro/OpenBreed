using OpenBreed.Common;
using OpenBreed.Common.Tiles;
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
    [Description("Tile set from Image"), Category("Appearance")]
    public class XmlTileSetFromImageEntry : XmlTileSetEntry, ITileSetFromImageEntry
    {
        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlElement("TileSize")]
        public int TileSize { get; set; }

        public override IEntry Copy()
        {
            return new XmlTileSetFromImageEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef,
                TileSize = TileSize
            };
        }
    }
}

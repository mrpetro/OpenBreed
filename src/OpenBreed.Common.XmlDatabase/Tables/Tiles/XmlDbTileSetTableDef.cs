using OpenBreed.Common.XmlDatabase.Items.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables.Tiles
{
    public class XmlDbTileSetTableDef : XmlDbTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("TileSetFromBLK", typeof(XmlTileSetFromBlkEntry)),
        XmlArrayItem("TileSetFromImage", typeof(XmlTileSetFromImageEntry))]
        public readonly List<XmlTileSetEntry> Items = new List<XmlTileSetEntry>();
    }
}

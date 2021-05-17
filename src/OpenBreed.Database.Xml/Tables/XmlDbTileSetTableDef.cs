using OpenBreed.Database.Xml.Items.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbTileSetTableDef : XmlDbTableDef
    {
        public const string NAME = "TileSets";

        [XmlIgnore]
        public override string Name => NAME;

        [XmlArray("Items"),
        XmlArrayItem("TileSetFromBLK", typeof(XmlTileSetFromBlkEntry)),
        XmlArrayItem("TileSetFromImage", typeof(XmlTileSetFromImageEntry))]
        public readonly List<XmlTileSetEntry> Items = new List<XmlTileSetEntry>();
    }
}

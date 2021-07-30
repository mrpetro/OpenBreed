using OpenBreed.Database.Xml.Items.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbTileAtlasTableDef : XmlDbTableDef
    {
        public const string NAME = "TileAtlases";

        [XmlIgnore]
        public override string Name => NAME;

        [XmlArray("Items"),
        XmlArrayItem("TileSetFromBLK", typeof(XmlDbTileAtlasFromBlk)),
        XmlArrayItem("TileSetFromImage", typeof(XmlDbTileAtlasFromImage))]
        public readonly List<XmlDbTileAtlas> Items = new List<XmlDbTileAtlas>();
    }
}

using OpenBreed.Database.Xml.Items.TileStamps;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbTileStampTableDef : XmlDbTableDef
    {
        public const string NAME = "TileStamps";

        [XmlIgnore]
        public override string Name => NAME;

        [XmlArray("Items"),
        XmlArrayItem("TileStamp", typeof(XmlDbTileStamp))]
        public readonly List<XmlDbTileStamp> Items = new List<XmlDbTileStamp>();
    }
}
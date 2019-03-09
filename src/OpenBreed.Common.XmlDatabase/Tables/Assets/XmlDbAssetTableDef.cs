using OpenBreed.Common.XmlDatabase.Items.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables.Assets
{
    public class XmlDbAssetTableDef : XmlDbTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("FileAsset", typeof(XmlFileAssetEntry)),
        XmlArrayItem("EPFArchiveFileAsset", typeof(XmlEPFArchiveFileAssetEntry))]
        public readonly List<XmlAssetEntry> Items = new List<XmlAssetEntry>();
    }
}

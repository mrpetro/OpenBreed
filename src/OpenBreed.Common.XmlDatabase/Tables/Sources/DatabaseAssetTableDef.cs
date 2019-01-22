using OpenBreed.Common.XmlDatabase.Items.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables.Sources
{
    public class DatabaseAssetTableDef : DatabaseTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("FileAsset", typeof(FileAssetDef)),
        XmlArrayItem("EPFArchiveFileAsset", typeof(EPFArchiveFileAssetDef))]
        public readonly List<AssetDef> Items = new List<AssetDef>();
    }
}

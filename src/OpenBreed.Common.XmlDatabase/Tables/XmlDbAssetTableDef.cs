using OpenBreed.Common.XmlDatabase.Items.Assets;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables
{
    public class XmlDbAssetTableDef : XmlDbTableDef
    {
        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("Asset", typeof(XmlAssetEntry))]
        public readonly List<XmlAssetEntry> Items = new List<XmlAssetEntry>();

        #endregion Public Fields
    }
}
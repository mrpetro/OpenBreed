using OpenBreed.Database.Xml.Items.Assets;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbAssetTableDef : XmlDbTableDef
    {
        public const string NAME = "Assets";

        [XmlIgnore]
        public override string Name => NAME;


        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("Asset", typeof(XmlAssetEntry))]
        public readonly List<XmlAssetEntry> Items = new List<XmlAssetEntry>();

        #endregion Public Fields
    }
}
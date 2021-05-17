using OpenBreed.Database.Xml.Items.DataSources;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbDataSourceTableDef : XmlDbTableDef
    {
        public const string NAME = "DataSources";

        [XmlIgnore]
        public override string Name => NAME;


        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("FileDataSource", typeof(XmlFileDataSourceEntry)),
        XmlArrayItem("EPFArchiveFileDataSource", typeof(XmlEPFArchiveFileDataSourceEntry))]
        public readonly List<XmlDataSourceEntry> Items = new List<XmlDataSourceEntry>();

        #endregion Public Fields
    }
}
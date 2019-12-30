using OpenBreed.Common.XmlDatabase.Items.DataSources;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables
{
    public class XmlDbDataSourceTableDef : XmlDbTableDef
    {
        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("FileDataSource", typeof(XmlFileDataSourceEntry)),
        XmlArrayItem("EPFArchiveFileDataSource", typeof(XmlEPFArchiveFileDataSourceEntry))]
        public readonly List<XmlDataSourceEntry> Items = new List<XmlDataSourceEntry>();

        #endregion Public Fields
    }
}
using OpenBreed.Common.Database.Items.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Tables.Sources
{
    public class DatabaseSourceTableDef : DatabaseTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("DirectoryFileSourceDef", typeof(DirectoryFileSourceDef)),
        XmlArrayItem("EPFArchiveFileSourceDef", typeof(EPFArchiveFileSourceDef))]
        public readonly List<SourceDef> Items = new List<SourceDef>();
    }
}

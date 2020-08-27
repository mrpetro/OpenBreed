using OpenBreed.Common.XmlDatabase.Items.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables
{
    public class XmlDbScriptTableDef : XmlDbTableDef
    {
        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("ScriptEmbedded", typeof(XmlScriptEmbeddedEntry)),
            ]
        public readonly List<XmlScriptEntry> Items = new List<XmlScriptEntry>();

        #endregion Public Fields
    }
}

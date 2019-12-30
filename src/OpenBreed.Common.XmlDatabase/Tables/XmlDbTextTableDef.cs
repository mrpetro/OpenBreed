using OpenBreed.Common.XmlDatabase.Items.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables
{
    public class XmlDbTextTableDef : XmlDbTableDef
    {
        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("TextEmbedded", typeof(XmlTextEmbeddedEntry)),
        XmlArrayItem("TextFromMAP", typeof(XmlTextFromMapEntry)),
            ]
        public readonly List<XmlTextEntry> Items = new List<XmlTextEntry>();

        #endregion Public Fields
    }
}

using OpenBreed.Database.Xml.Items.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbScriptTableDef : XmlDbTableDef
    {
        public const string NAME = "Scripts";

        [XmlIgnore]
        public override string Name => NAME;

        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("ScriptEmbedded", typeof(XmlDbScriptEmbedded)),
        XmlArrayItem("ScriptFromFile", typeof(XmlDbScriptFromFile)),]
        public readonly List<XmlDbScript> Items = new List<XmlDbScript>();

        #endregion Public Fields
    }
}

using OpenBreed.Common.XmlDatabase.Items.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables
{
    public class DatabaseSoundTableDef : XmlDbTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("Sound", typeof(XmlSoundEntry))]
        public readonly List<XmlSoundEntry> Items = new List<XmlSoundEntry>();
    }
}

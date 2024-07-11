using OpenBreed.Database.Xml.Items.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbSoundTableDef : XmlDbTableDef
    {
        public const string NAME = "Sounds";

        [XmlIgnore]
        public override string Name => NAME;

        [XmlArray("Items"),
        XmlArrayItem("Sound", typeof(XmlDbSound))]
        public readonly List<XmlDbSound> Items = new List<XmlDbSound>();
    }
}

using OpenBreed.Common.XmlDatabase.Items.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables.Images
{
    public class DatabaseSoundTableDef : DatabaseTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("Sound", typeof(SoundDef))]
        public readonly List<SoundDef> Items = new List<SoundDef>();
    }
}

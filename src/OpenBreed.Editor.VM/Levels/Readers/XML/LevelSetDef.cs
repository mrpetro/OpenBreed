using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Editor.VM.Levels.Readers.XML
{
    [Serializable]
    public class LevelSetDef
    {
        [XmlAttribute]
        public string Name { get; set; }

        public List<LevelDef> LevelDefs { get; set; }
    }
}

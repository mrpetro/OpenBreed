using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Editor.VM.Database.Resources
{
    [Serializable]
    public class ResourceDef
    {
        [XmlAttribute]
        public string Name { get; set; }
    }
}

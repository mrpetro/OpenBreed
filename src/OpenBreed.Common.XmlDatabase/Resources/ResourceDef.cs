using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Resources
{
    [Serializable]
    public class ResourceDef
    {
        [XmlAttribute]
        public string Name { get; set; }
    }
}

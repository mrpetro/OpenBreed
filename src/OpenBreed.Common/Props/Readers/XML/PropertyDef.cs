using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.Props.Readers.XML
{
    [Serializable]
    public class PropertyDef
    {
        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public bool Visibility { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public string ImagePath { get; set; }
    }
}

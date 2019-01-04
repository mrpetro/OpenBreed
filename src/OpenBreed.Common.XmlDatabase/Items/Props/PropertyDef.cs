using OpenBreed.Common.Props;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Props
{
    [Serializable]
    public class PropertyDef : IPropertyEntry
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

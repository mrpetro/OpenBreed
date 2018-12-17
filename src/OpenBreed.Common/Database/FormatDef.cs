using OpenBreed.Common.Database.Items.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database
{
    public class FormatDef
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlArrayItem(ElementName = "Parameter")]
        public readonly List<FormatParameterDef> Parameters = new List<FormatParameterDef>();
    }
}

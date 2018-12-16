using OpenBreed.Common.Database.Items.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Items.Sprites
{
    public class SpriteSetDef : DatabaseItemDef
    {
        public string SourceRef { get; set; }
        public string Format { get; set; }

        [XmlArrayItem(ElementName = "Parameter")]
        public readonly List<SourceParameterDef> Parameters = new List<SourceParameterDef>();
    }
}

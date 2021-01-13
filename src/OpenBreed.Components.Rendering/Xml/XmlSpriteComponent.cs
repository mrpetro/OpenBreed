using OpenBreed.Core.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Components.Rendering.Xml
{
    [XmlRoot("Sprite")]
    public class XmlSpriteComponent : XmlComponentTemplate, ISpriteComponentTemplate
    {
        public string AtlasName { get; set; }
        public int ImageIndex { get; set; }
        public int Order { get; set; }
    }
}

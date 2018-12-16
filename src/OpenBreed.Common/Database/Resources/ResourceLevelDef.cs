using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Resources
{
    [Serializable]
    public class ResourceLevelDef : ResourceDef
    {
        [XmlAttribute]
        public int Id { get; set; }

        public string MapResourceRef { get; set; }
        public string TileSetResourceRef { get; set; }
        public string PropertySetResourceRef { get; set; }
        [XmlArray("SpriteSetRefs"),
        XmlArrayItem("SpriteSetRef", typeof(string))]
        public List<string> SpriteSetRefs { get; set; }


        public static ResourceLevelDef CreateDefault()
        {
            var resourceLevelDef = new ResourceLevelDef();
            resourceLevelDef.MapResourceRef = "%DEFAULT%";
            resourceLevelDef.TileSetResourceRef = "%DEFAULT%";
            resourceLevelDef.PropertySetResourceRef = "%DEFAULT%";
            resourceLevelDef.SpriteSetRefs = new List<string>(new string[] { "%DEFAULT%" });
            return resourceLevelDef;
        }
    }
}

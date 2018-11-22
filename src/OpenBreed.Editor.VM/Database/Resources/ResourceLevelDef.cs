using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Editor.VM.Database.Resources
{
    [Serializable]
    public class ResourceLevelDef : ResourceDef
    {
        [XmlAttribute]
        public int Id { get; set; }

        public string MapResourceRef { get; set; }
        public string TileSetResourceRef { get; set; }
        public string PropertySetResourceRef { get; set; }
        [XmlArray("SpriteSetResourceRefs"),
        XmlArrayItem("SpriteSetResourceRef", typeof(string))]
        public List<string> SpriteSetResourceRefs { get; set; }


        public static ResourceLevelDef CreateDefault()
        {
            var resourceLevelDef = new ResourceLevelDef();
            resourceLevelDef.MapResourceRef = "%DEFAULT%";
            resourceLevelDef.TileSetResourceRef = "%DEFAULT%";
            resourceLevelDef.PropertySetResourceRef = "%DEFAULT%";
            resourceLevelDef.SpriteSetResourceRefs = new List<string>(new string[] { "%DEFAULT%" });
            return resourceLevelDef;
        }
    }
}

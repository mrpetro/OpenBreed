using OpenBreed.Common.Sprites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Sprites
{
    [Serializable]
    [Description("Sprite set from Image"), Category("Appearance")]
    public class XmlSpriteSetFromImageEntry : XmlSpriteSetEntry, ISpriteSetFromImageEntry
    {
        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlArray("Sprites")]
        [XmlArrayItem(ElementName = "SpriteCoords")]
        public List<XmlSpriteCoords> XmlSprites { get; set; }

        [XmlIgnore]
        public List<ISpriteCoords> Sprites
        {
            get
            {
                return XmlSprites.Cast<ISpriteCoords>().ToList();
            }
        }

        public override IEntry Copy()
        {
            return new XmlSpriteSetFromImageEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef
            };
        }
    }
}

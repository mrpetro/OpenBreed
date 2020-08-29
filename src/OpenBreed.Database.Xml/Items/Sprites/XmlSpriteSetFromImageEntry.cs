using OpenBreed.Common;
using OpenBreed.Common.Sprites;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Sprites
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
        public ReadOnlyCollection<ISpriteCoords> Sprites
        {
            get
            {
                return new ReadOnlyCollection<ISpriteCoords>(XmlSprites.Cast<ISpriteCoords>().ToList());
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

        public void ClearCoords()
        {
            XmlSprites.Clear();
        }

        public void AddCoords(int x, int y, int width, int height)
        {
            XmlSprites.Add(new XmlSpriteCoords() { X = x, Y = y, Width = width, Height = height });
        }
    }
}

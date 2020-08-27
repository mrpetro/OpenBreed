using OpenBreed.Common;
using OpenBreed.Common.Sprites;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Sprites
{
    [Serializable]
    [Description("Sprite set from SPR"), Category("Appearance")]
    public class XmlSpriteSetFromSprEntry : XmlSpriteSetEntry, ISpriteSetFromSprEntry
    {
        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        public override IEntry Copy()
        {
            return new XmlSpriteSetFromSprEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef
            };
        }
    }
}

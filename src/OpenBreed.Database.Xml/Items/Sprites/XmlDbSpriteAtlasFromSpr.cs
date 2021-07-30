using OpenBreed.Common;
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
    [Description("Sprite atlas from SPR"), Category("Appearance")]
    public class XmlDbSpriteAtlasFromSpr : XmlDbSpriteAtlas, IDbSpriteAtlasFromSpr
    {
        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        public override IDbEntry Copy()
        {
            return new XmlDbSpriteAtlasFromSpr()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef
            };
        }
    }
}

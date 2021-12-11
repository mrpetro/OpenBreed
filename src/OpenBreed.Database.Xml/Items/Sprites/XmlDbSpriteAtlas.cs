using OpenBreed.Database.Xml.Items.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;

namespace OpenBreed.Database.Xml.Items.Sprites
{
    [Serializable]
    public abstract class XmlDbSpriteAtlas : XmlDbEntry, IDbSpriteAtlas
    {
        #region Public Properties

        [XmlArray("PaletteRefs"),
        XmlArrayItem("PaletteRef", typeof(string))]
        public List<string> PaletteRefs { get; } = new List<string>();

        public override IDbEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}

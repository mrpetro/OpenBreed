using OpenBreed.Common.XmlDatabase.Items.Assets;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;

namespace OpenBreed.Common.XmlDatabase.Items.Sprites
{
    [Serializable]
    public abstract class XmlSpriteSetEntry : XmlDbEntry, ISpriteSetEntry
    {
        #region Public Properties

        [XmlArray("PaletteRefs"),
        XmlArrayItem("PaletteRef", typeof(string))]
        public List<string> PaletteRefs { get; } = new List<string>();

        public override IEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}

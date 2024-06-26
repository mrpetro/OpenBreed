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
using OpenBreed.Database.Xml.Items.Scripts;

namespace OpenBreed.Database.Xml.Items.Sprites
{
    [Serializable]
    public abstract class XmlDbSpriteAtlas : XmlDbEntry, IDbSpriteAtlas
    {
        #region Protected Constructors

        protected XmlDbSpriteAtlas()
        {
        }

        protected XmlDbSpriteAtlas(XmlDbSpriteAtlas other) : base(other)
        {
            PaletteRefs = other.PaletteRefs.ToList();
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlArray("PaletteRefs"),
        XmlArrayItem("PaletteRef", typeof(string))]
        public List<string> PaletteRefs { get; } = new List<string>();

        #endregion Public Properties
    }
}
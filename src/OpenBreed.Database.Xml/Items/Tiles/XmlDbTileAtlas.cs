using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Xml.Items.Texts;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Tiles
{
    [Serializable]
    public abstract class XmlDbTileAtlas : XmlDbEntry, IDbTileAtlas
    {
        #region Protected Constructors

        protected XmlDbTileAtlas()
        {
        }

        protected XmlDbTileAtlas(XmlDbTileAtlas other) : base(other)
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlArray("PaletteRefs"),
        XmlArrayItem("PaletteRef", typeof(string))]
        public List<string> PaletteRefs { get; } = new List<string>();

        #endregion Public Properties
    }
}
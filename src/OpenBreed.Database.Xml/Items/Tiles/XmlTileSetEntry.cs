using OpenBreed.Common.Tiles;
using OpenBreed.Database.Interface.Items.Tiles;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Tiles
{
    [Serializable]
    public abstract class XmlTileSetEntry : XmlDbEntry, ITileSetEntry
    {
        [XmlArray("PaletteRefs"),
        XmlArrayItem("PaletteRef", typeof(string))]
        public List<string> PaletteRefs { get; } = new List<string>();
    }
}
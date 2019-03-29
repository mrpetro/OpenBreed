using OpenBreed.Common.Tiles;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Tiles
{
    [Serializable]
    public abstract class XmlTileSetEntry : XmlDbEntry, ITileSetEntry
    {
        [XmlArray("PaletteRefs"),
        XmlArrayItem("PaletteRef", typeof(string))]
        public List<string> PaletteRefs { get; } = new List<string>();
    }
}
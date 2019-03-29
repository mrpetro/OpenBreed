﻿using OpenBreed.Common.XmlDatabase.Items.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables.Palettes
{
    public class XmlDbPaletteTableDef : XmlDbTableDef
    {
        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("PaletteFromBinary", typeof(XmlPaletteFromBinaryEntry)),
        XmlArrayItem("PaletteFromMAP", typeof(XmlPaletteFromMapEntry))]
        public readonly List<XmlPaletteEntry> Items = new List<XmlPaletteEntry>();

        #endregion Public Fields
    }
}

﻿using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Palettes
{
    [Serializable]
    [Description("Palette from MAP"), Category("Appearance")]
    public class XmlPaletteFromMapEntry : XmlPaletteEntry, IPaletteFromMapEntry
    {
        [XmlElement("BlockName")]
        public string BlockName { get; set; }

        public override IEntry Copy()
        {
            return new XmlPaletteFromMapEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef,
                BlockName = this.BlockName
            };
        }
    }
}

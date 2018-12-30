﻿using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Items.Levels
{
    [Serializable]
    public class LevelDef : DatabaseItemDef, ILevelEntity
    {
        #region Public Properties

        [XmlIgnore]
        public IFormatEntity Format { get; set; }

        [XmlElement("Format")]
        public FormatDef FormatDef
        {
            get
            {
                return (FormatDef)Format;
            }

            set
            {
                Format = value;
            }
        }

        public long Id { get; set; }
        [XmlArray("PaletteRefs"),
        XmlArrayItem("PaletteRef", typeof(string))]
        public List<string> PaletteRefs { get; } = new List<string>();

        public string PropertySetRef { get; set; }
        public string SourceRef { get; set; }

        [XmlArray("SpriteSetRefs"),
        XmlArrayItem("SpriteSetRef", typeof(string))]
        public List<string> SpriteSetRefs { get; } = new List<string>();

        public string TileSetRef { get; set; }

        #endregion Public Properties

    }
}

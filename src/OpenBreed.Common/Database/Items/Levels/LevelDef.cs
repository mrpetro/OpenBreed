﻿using OpenBreed.Common.Database.Items.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Items.Levels
{
    [Serializable]
    public class LevelDef : DatabaseItemDef
    {
        [XmlAttribute]
        public int Id { get; set; }

        public string SourceRef { get; set; }
        public string Format { get; set; }
        [XmlArrayItem(ElementName = "Parameter")]
        public readonly List<SourceParameterDef> Parameters = new List<SourceParameterDef>();


        public string TileSetRef { get; set; }
        public string PropertySetRef { get; set; }
        [XmlArray("SpriteSetRefs"),
        XmlArrayItem("SpriteSetRef", typeof(string))]
        public List<string> SpriteSetRefs { get; set; }
    }
}

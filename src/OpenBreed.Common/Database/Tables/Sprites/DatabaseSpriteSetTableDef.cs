﻿using OpenBreed.Common.Database.Items.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Tables.Sprites
{
    public class DatabaseSpriteSetTableDef : DatabaseTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("SpriteSet", typeof(SpriteSetDef))]
        public readonly List<SpriteSetDef> Items = new List<SpriteSetDef>();
    }
}

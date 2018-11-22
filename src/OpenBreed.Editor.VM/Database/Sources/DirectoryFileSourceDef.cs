﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Editor.VM.Database.Sources
{
    [Serializable]
    public class DirectoryFileSourceDef : SourceDef
    {
        [XmlAttribute]
        public string DirectoryPath { get; set; }
    }
}

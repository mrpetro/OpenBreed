using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Sources
{
    [Serializable]
    public class DirectoryFileAssetDef : AssetDef, IDirectoryFileAssetEntry
    {
        [XmlAttribute]
        public string FileName { get; set; }

        [XmlAttribute]
        public string DirectoryPath { get; set; }
    }
}

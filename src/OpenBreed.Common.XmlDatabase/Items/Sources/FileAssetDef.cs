using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Sources
{
    [Serializable]
    public class FileAssetDef : AssetDef, IFileAssetEntry
    {
        [XmlAttribute]
        public string FilePath { get; set; }

        public override IEntry Copy()
        {
            return new FileAssetDef()
            {
                Id = this.Id,
                FilePath = this.FilePath
            };
        }
    }
}

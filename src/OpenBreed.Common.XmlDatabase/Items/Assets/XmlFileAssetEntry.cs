using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Assets
{
    [Serializable]
    [Description("File"), Category("Appearance")]
    public class XmlFileAssetEntry : XmlAssetEntry, IFileAssetEntry
    {
        [XmlAttribute]
        public string FilePath { get; set; }

        public override IEntry Copy()
        {
            return new XmlFileAssetEntry()
            {
                Id = this.Id,
                FilePath = this.FilePath
            };
        }
    }
}

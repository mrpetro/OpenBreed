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
    [Description("EPF Archive Item"), Category("Appearance")]
    public class XmlEPFArchiveFileAssetEntry : XmlAssetEntry, IEPFArchiveAssetEntry
    {
        [XmlAttribute]
        public string EntryName { get; set; }

        [XmlAttribute]
        public string ArchivePath { get; set; }

        public override IEntry Copy()
        {
            return new XmlEPFArchiveFileAssetEntry()
            {
                Id = this.Id,
                EntryName = this.EntryName,
                ArchivePath = this.ArchivePath
            };
        }
    }
}

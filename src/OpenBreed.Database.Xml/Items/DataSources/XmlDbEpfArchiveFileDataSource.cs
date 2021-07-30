using OpenBreed.Common;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.DataSources
{
    [Serializable]
    [Description("EPF Archive Item"), Category("Appearance")]
    public class XmlDbEpfArchiveFileDataSource : XmlDbDataSource, IDbEpfArchiveDataSource
    {
        #region Public Properties

        [XmlAttribute]
        public string EntryName { get; set; }

        [XmlAttribute]
        public string ArchivePath { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy()
        {
            return new XmlDbEpfArchiveFileDataSource()
            {
                Id = this.Id,
                EntryName = this.EntryName,
                ArchivePath = this.ArchivePath
            };
        }

        #endregion Public Methods
    }
}
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Xml.Items.Palettes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.DataSources
{
    [Serializable]
    [Description("EPF Archive Item"), Category("Appearance")]
    public class XmlDbEpfArchiveFileDataSource : XmlDbDataSource, IDbEpfArchiveDataSource
    {
        #region Public Constructors

        public XmlDbEpfArchiveFileDataSource()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbEpfArchiveFileDataSource(XmlDbEpfArchiveFileDataSource other) : base(other)
        {
            EntryName = other.EntryName;
            ArchivePath = other.ArchivePath;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlAttribute]
        public string EntryName { get; set; }

        [XmlAttribute]
        public string ArchivePath { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbEpfArchiveFileDataSource(this);


        #endregion Public Methods
    }
}
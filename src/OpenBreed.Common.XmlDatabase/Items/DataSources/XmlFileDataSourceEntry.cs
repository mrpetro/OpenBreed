using OpenBreed.Common.DataSources;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.DataSources
{
    [Serializable]
    [Description("File"), Category("Appearance")]
    public class XmlFileDataSourceEntry : XmlDataSourceEntry, IFileDataSourceEntry
    {
        #region Public Properties

        [XmlAttribute]
        public string FilePath { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntry Copy()
        {
            return new XmlFileDataSourceEntry()
            {
                Id = this.Id,
                FilePath = this.FilePath
            };
        }

        #endregion Public Methods
    }
}
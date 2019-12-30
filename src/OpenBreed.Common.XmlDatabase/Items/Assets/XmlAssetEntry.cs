using OpenBreed.Common.Assets;
using OpenBreed.Common.Formats;
using System;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Assets
{
    [Serializable]
    public class XmlAssetEntry : XmlDbEntry, IAssetEntry
    {
        #region Public Properties

        [XmlIgnore]
        public IFormatEntry Format { get; set; }

        [XmlAttribute("DataSourceRef")]
        public string DataSourceRef { get; set; }

        [XmlElement("Format")]
        public XmlFormatEntry FormatDef
        {
            get
            {
                return (XmlFormatEntry)Format;
            }

            set
            {
                Format = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override IEntry Copy()
        {
            return new XmlAssetEntry()
            {
                Id = this.Id,
                DataSourceRef = this.DataSourceRef
            };
        }

        #endregion Public Methods
    }
}
using OpenBreed.Common.Data2;
using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Data
{
    [Serializable]
    [Description("Data"), Category("Appearance")]
    public class XmlDataEntry : XmlDbEntry, IDataEntry
    {
        #region Public Properties

        [XmlIgnore]
        public IFormatEntry Format { get; set; }

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

        public string AssetRef { get; set; }

        public override IEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties

    }
}

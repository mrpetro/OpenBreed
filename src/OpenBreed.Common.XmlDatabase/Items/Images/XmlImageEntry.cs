using OpenBreed.Common.XmlDatabase.Items.Assets;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;

namespace OpenBreed.Common.XmlDatabase.Items.Images
{
    [Serializable]
    [Description("Image"), Category("Appearance")]
    public class XmlImageEntry : XmlDbEntry, IImageEntry
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

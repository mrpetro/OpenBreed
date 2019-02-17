using OpenBreed.Common.Formats;
using OpenBreed.Common.Sounds;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Sounds
{
    [Serializable]
    [Description("Sound"), Category("Appearance")]
    public class XmlSoundEntry : XmlDbEntry, ISoundEntry
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

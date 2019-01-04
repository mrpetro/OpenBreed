using OpenBreed.Common.Formats;
using OpenBreed.Common.Sounds;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Sounds
{
    public class SoundDef : DatabaseItemDef, ISoundEntry
    {
        #region Public Properties

        [XmlIgnore]
        public IFormatEntry Format { get; set; }

        [XmlElement("Format")]
        public FormatDef FormatDef
        {
            get
            {
                return (FormatDef)Format;
            }

            set
            {
                Format = value;
            }
        }

        public long Id { get; set; }
        public string SourceRef { get; set; }

        #endregion Public Properties
    }
}

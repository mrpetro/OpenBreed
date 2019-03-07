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

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        public override IEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}

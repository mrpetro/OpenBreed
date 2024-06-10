using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sounds;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Sounds
{
    [Serializable]
    [Description("Sound"), Category("Appearance")]
    public class XmlDbSound : XmlDbEntry, IDbSound
    {
        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlElement("SampleRate")]
        public int SampleRate { get; set; }

        [XmlElement("BitsPerSample")]
        public int BitsPerSample { get; set; }

        [XmlElement("Channels")]
        public int Channels { get; set; }

        public override IDbEntry Copy()
        {
            return new XmlDbSound
            {
                DataRef = this.DataRef,
                SampleRate = this.SampleRate,
                BitsPerSample = this.BitsPerSample,
                Channels = this.Channels
            };
        }

        #endregion Public Properties
    }
}

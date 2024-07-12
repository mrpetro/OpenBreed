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
        #region Public Constructors

        public XmlDbSound()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbSound(XmlDbSound other) : base(other)
        {
            DataRef = other.DataRef;
            SampleRate = other.SampleRate;
            BitsPerSample = other.BitsPerSample;
            Channels = other.Channels;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlElement("SampleRate")]
        public int SampleRate { get; set; }

        [XmlElement("BitsPerSample")]
        public int BitsPerSample { get; set; }

        [XmlElement("Channels")]
        public int Channels { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbSound(this);

        #endregion Public Methods
    }
}
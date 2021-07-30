using OpenBreed.Common;
using OpenBreed.Common.Formats;
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

        public override IDbEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}

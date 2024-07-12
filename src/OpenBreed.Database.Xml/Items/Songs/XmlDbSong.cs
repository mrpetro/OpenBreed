using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Songs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Songs
{
    [Serializable]
    [Description("Song"), Category("Appearance")]
    public class XmlDbSong : XmlDbEntry, IDbSong
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

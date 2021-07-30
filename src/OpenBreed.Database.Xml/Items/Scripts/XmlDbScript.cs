using OpenBreed.Common.Formats;
using OpenBreed.Database.Interface.Items.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Scripts
{
    [Serializable]

    public abstract class XmlDbScript : XmlDbEntry, IDbScript
    {
        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        #endregion Public Properties
    }
}

using OpenBreed.Common.Formats;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Texts
{
    [Serializable]

    public abstract class XmlScriptEntry : XmlDbEntry, IScriptEntry
    {
        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        #endregion Public Properties
    }
}

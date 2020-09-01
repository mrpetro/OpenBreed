using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Texts
{
    [Serializable]
    [Description("Script embedded"), Category("Appearance")]
    public class XmlScriptEmbeddedEntry : XmlScriptEntry, IScriptEmbeddedEntry
    {
        #region Public Properties

        [XmlElement("Script")]
        public string Script { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntry Copy()
        {
            return new XmlScriptEmbeddedEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef,
                Script = this.Script
            };
        }

        #endregion Public Methods
    }
}

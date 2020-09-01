using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Texts;
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
    [Description("Text embedded"), Category("Appearance")]
    public class XmlTextEmbeddedEntry : XmlTextEntry, ITextEmbeddedEntry
    {
        #region Public Properties

        [XmlElement("Text")]
        public string Text { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntry Copy()
        {
            return new XmlTextEmbeddedEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef,
                Text = this.Text
            };
        }

        #endregion Public Methods
    }
}

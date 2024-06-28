using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Xml.Items.Sprites;
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
    public class XmlDbTextEmbedded : XmlDbText, IDbTextEmbedded
    {
        #region Public Constructors

        public XmlDbTextEmbedded()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbTextEmbedded(XmlDbTextEmbedded other) : base(other)
        {
            Text = other.Text;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("Text")]
        public string Text { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbTextEmbedded(this);

        #endregion Public Methods
    }
}
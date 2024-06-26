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

namespace OpenBreed.Database.Xml.Items.Scripts
{
    [Serializable]
    [Description("Script embedded"), Category("Appearance")]
    public class XmlDbScriptEmbedded : XmlDbScript, IDbScriptEmbedded
    {
        #region Public Constructors

        public XmlDbScriptEmbedded()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbScriptEmbedded(XmlDbScriptEmbedded other) : base(other)
        {
            Script = this.Script;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("Script")]
        public string Script { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbScriptEmbedded(this);

        #endregion Public Methods
    }
}
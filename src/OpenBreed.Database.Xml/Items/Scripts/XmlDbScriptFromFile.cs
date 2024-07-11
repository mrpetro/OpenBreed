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
    [Description("Script from file"), Category("Appearance")]
    public class XmlDbScriptFromFile : XmlDbScript, IDbScriptFromFile
    {
        #region Public Constructors

        public XmlDbScriptFromFile()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbScriptFromFile(XmlDbScriptFromFile other) : base(other)
        {
            DataRef = this.DataRef;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbScriptFromFile(this);

        #endregion Public Methods
    }
}